using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using sms.backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using sms.backend.Migrations;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SchoolContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                SchoolContext context,
                                IConfiguration configuration)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
        }

        // POST: api/account/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int x = 0;

                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (!result.Succeeded)
                    return BadRequest(result.Errors);

                // Optionally assign a role if provided

                if (!await _roleManager.RoleExistsAsync(model.Role))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(model.Role));
                    if (!roleResult.Succeeded)
                        return BadRequest(roleResult.Errors);
                }

                var addRoleResult = await _userManager.AddToRoleAsync(user, model.Role);
                if (!addRoleResult.Succeeded)
                    return BadRequest(addRoleResult.Errors);


                return Ok(new { user.Id, user.Email });
            }
        

        [HttpGet("{userType}/unlinked")]
        public async Task<IActionResult> GetUnlinkedUsers(string userType)
        {
            if (string.IsNullOrEmpty(userType))
                return BadRequest("User type is required.");

            // Retrieve all Identity users.
            var allUsers = await _userManager.Users.ToListAsync();

            if (userType.ToLower() == "student")
            {
                // Get Identity user IDs already linked to a Student.
                var linkedUserIds = await _context.Students
                    .Where(s => s.UserId != null)
                    .Select(s => s.UserId)
                    .ToListAsync();
                var unlinkedUsers = allUsers
                    .Where(u => !linkedUserIds.Contains(u.Id) && _userManager.IsInRoleAsync(u, "Student").Result)
                    .Select(u => new { u.Id, u.Email });
                return Ok(unlinkedUsers);
            }
            else if (userType.ToLower() == "teacher")
            {
                // Get Identity user IDs already linked to a Staff member.
                var linkedUserIds = await _context.Staff
                    .Where(s => s.UserId != null)
                    .Select(s => s.UserId)
                    .ToListAsync();
                var unlinkedUsers = allUsers
                    .Where(u => !linkedUserIds.Contains(u.Id) && _userManager.IsInRoleAsync(u, "Teacher").Result)
                    .Select(u => new { u.Id, u.Email });
                return Ok(unlinkedUsers);
            }
            else if (userType.ToLower() == "parent")
            {
                // Get Identity user IDs already linked as a Parent (in StudentParent table)
                var linkedUserIds = await _context.Parents
                    .Select(sp => sp.UserId)
                    .Distinct()
                    .ToListAsync();
                var unlinkedUsers = allUsers
                    .Where(u => !linkedUserIds.Contains(u.Id) && _userManager.IsInRoleAsync(u, "Parent").Result)
                    .Select(u => new { u.Id, u.Email });
                return Ok(unlinkedUsers);
            }
            else
            {
                return BadRequest("Invalid user type. Valid values are: student, teacher, parent.");
            }
        }

        // POST: api/account/logout
        [Authorize]
        [HttpPost("logout")]
        public IActionResult Logout()
        {

            return Ok(new { message = "Logged out successfully." });
        }

        // POST: api/account/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            List<string> tokens = new List<string>();

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                    return Unauthorized();

                var roles = await _userManager.GetRolesAsync(user);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                    if (role.ToLower() == "student")
                    {
                        var student = _context.Students.FirstOrDefault(s => s.UserId == user.Id);
                        if (student != null)
                        {
                            var studentId = student.StudentId;
                            claims.Add(new Claim("studentId", studentId.ToString()));
                        }
                    }
                    if (role.ToLower() == "teacher")
                    {
                        var teacher = _context.Staff.FirstOrDefault(s => s.UserId == user.Id);
                        if (teacher != null)
                        {
                            var teacherId = teacher.StaffId;
                            claims.Add(new Claim("teacherId", teacherId.ToString()));
                        }
                    }

                    if (role.ToLower() == "parent")
                    {
                        var parent = _context.Parents.FirstOrDefault(s => s.UserId == user.Id);
                        if (parent != null)
                        {
                            var teacherId = parent.ParentId;
                            claims.Add(new Claim("parentId", teacherId.ToString()));
                        }
                    }
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddDays(7),
                    signingCredentials: creds
                );

                tokens.Add(new JwtSecurityTokenHandler().WriteToken(token));

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }

        // POST: api/account/resetpassword
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                // For security, don't reveal that the user doesn't exist
                return Ok("If the user exists, the password reset has been processed.");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            return Ok("Password reset successful.");
        }
    }
}
