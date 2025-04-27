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
            List<string> linkedUserIds = new();

            if (userType.Equals("student", StringComparison.OrdinalIgnoreCase))
            {
                // Get Identity user IDs already linked to a Student.
                linkedUserIds = await _context.Students
                    .Where(s => s.UserId != null)
                    .Select(s => s.UserId)
                    .ToListAsync();

                var unlinkedStudents = new List<IdentityUser>();

                foreach (var user in allUsers)
                {
                    bool isStudent = await _userManager.IsInRoleAsync(user, "Student");
                    if (isStudent && !linkedUserIds.Contains(user.Id))
                    {
                        unlinkedStudents.Add(user);
                    }
                }

                // For each unlinked Identity user, also get the numeric StudentId (if exists) from Students.
                var result = unlinkedStudents.Select(u => new
                {
                    IdentityUserId = u.Id,
                    Email = u.Email,
                    NumericId = _context.Students.FirstOrDefault(s => s.UserId == u.Id)?.StudentId ?? 0
                }).ToList();

                return Ok(result);
            }
            else if (userType.Equals("teacher", StringComparison.OrdinalIgnoreCase))
            {
                // Get Identity user IDs already linked to a Staff member.
                linkedUserIds = await _context.Staff
                    .Where(s => s.UserId != null)
                    .Select(s => s.UserId)
                    .ToListAsync();

                var unlinkedTeachers = new List<IdentityUser>();

                foreach (var user in allUsers)
                {
                    bool isTeacher = await _userManager.IsInRoleAsync(user, "Teacher");
                    if (isTeacher && !linkedUserIds.Contains(user.Id))
                    {
                        unlinkedTeachers.Add(user);
                    }
                }

                // For each unlinked Identity user, also get the numeric StaffId from Staff.
                var result = unlinkedTeachers.Select(u => new
                {
                    IdentityUserId = u.Id,
                    Email = u.Email,
                    NumericId = _context.Staff.FirstOrDefault(s => s.UserId == u.Id)?.StaffId ?? 0
                }).ToList();

                return Ok(result);
            }
            else if (userType.Equals("parent", StringComparison.OrdinalIgnoreCase))
            {
                // Get Identity user IDs already linked as a Parent.
                linkedUserIds = await _context.Parents
                    .Where(p => p.UserId != null)
                    .Select(p => p.UserId)
                    .Distinct()
                    .ToListAsync();

                var unlinkedParents = new List<IdentityUser>();

                foreach (var user in allUsers)
                {
                    bool isParent = await _userManager.IsInRoleAsync(user, "Parent");
                    if (isParent && !linkedUserIds.Contains(user.Id))
                    {
                        unlinkedParents.Add(user);
                    }
                }

                // For each unlinked Identity user, also get the numeric ParentId from Parents.
                var result = unlinkedParents.Select(u => new
                {
                    IdentityUserId = u.Id,
                    Email = u.Email,
                    NumericId = _context.Parents.FirstOrDefault(p => p.UserId == u.Id)?.ParentId ?? 0
                }).ToList();

                return Ok(result);
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


        // New endpoint to get user account info via user ID
        [Authorize]
        [HttpGet("userinfo/{id}")]
        public async Task<IActionResult> GetUserAccountInfo(string id)
        {
            if (string.IsNullOrEmpty(id))
                return BadRequest("User ID is required.");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return NotFound("User not found.");

            var roles = await _userManager.GetRolesAsync(user);

            // Optionally, you can return additional properties here as needed.
            var accountInfo = new
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                Roles = roles
            };

            return Ok(accountInfo);
        }


    }
}
