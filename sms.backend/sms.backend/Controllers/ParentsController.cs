using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using sms.backend.Data;
using sms.backend.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParentsController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<ParentsController> _logger;

        public ParentsController(SchoolContext context, ILogger<ParentsController> logger)
        {
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

        // PUT: api/parents/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParent(int id, Parent parent)
        {
            if (id != parent.ParentId)
            {
                return BadRequest("Parent ID mismatch");
            }

            _context.Entry(parent).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
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
