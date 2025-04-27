using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sms.backend.Data;
using sms.backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParentsController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<ParentsController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public ParentsController(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            SchoolContext context,
            ILogger<ParentsController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _context = context;
            _logger = logger;
        }

        // GET: api/parents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parent>>> GetParents()
        {
            _logger.LogInformation("Getting all parents");
            return await _context.Parents.Include(p => p.StudentParents).ToListAsync();
        }

        // GET: api/parents/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Parent>> GetParent(int id)
        {
            _logger.LogInformation("Getting parent with ID: {Id}", id);
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                _logger.LogWarning("Parent with ID: {Id} not found", id);
                return NotFound();
            }
            return parent;
        }

        // POST: api/parents
        [HttpPost]
        public async Task<ActionResult<Parent>> PostParent(Parent parent)
        {
            _logger.LogInformation("Creating new parent");
            _context.Parents.Add(parent);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetParent), new { id = parent.ParentId }, parent);
        }

        // POST: api/parents/{id}
        // Update endpoint using POST instead of PUT.
        [HttpPost("{id}")]
        public async Task<IActionResult> EditParent(int id, Parent parent)
        {
            if (id != parent.ParentId)
            {
                return BadRequest("Parent ID mismatch");
            }

            // Fetch the existing parent entity.
            var existingParent = await _context.Parents.FirstOrDefaultAsync(p => p.ParentId == id);
            if (existingParent == null)
            {
                _logger.LogWarning("Parent with ID {Id} not found during update", id);
                return NotFound();
            }

            // Check if another Identity user already exists with this email.
            // (Exclude the current parent by checking that existingParent.UserId matches the found Identity user's Id.)
            var otherParent = await _context.Parents
                        .FirstOrDefaultAsync(p => p.UserId == parent.UserId && p.ParentId != parent.ParentId);
            if (otherParent != null)
            {
                // Another parent is already using the same identity user
                _logger.LogWarning("Another parent is linked to userId {UserId}", parent.UserId);
                return Conflict("Another parent is already linked to that user.");
            }

            // Map only the editable properties.
            existingParent.FirstName = parent.FirstName;
            existingParent.LastName = parent.LastName;
            existingParent.Email = parent.Email;
            existingParent.UserId = parent.UserId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error while updating Parent with ID {Id}", id);
                if (!ParentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(500, "A concurrency error occurred while updating the parent.");
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error updating Parent with ID {Id}", id);
                return StatusCode(500, "Internal server error while updating the parent.");
            }
            return NoContent();
        }

        // DELETE: api/parents/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParent(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }
            _context.Parents.Remove(parent);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool ParentExists(int id)
        {
            return _context.Parents.Any(e => e.ParentId == id);
        }
    }
}
