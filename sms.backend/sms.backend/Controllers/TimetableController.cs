using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TimetableController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<TimetableController> _logger;

        public TimetableController(SchoolContext context, ILogger<TimetableController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Timetable>>> GetTimetables()
        {
            _logger.LogInformation("Getting all timetables");
            return await _context.Timetables.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Timetable>> GetTimetable(int id)
        {
            _logger.LogInformation("Getting timetable with ID: {Id}", id);
            var timetable = await _context.Timetables.FindAsync(id);
            if (timetable == null)
            {
                _logger.LogWarning("Timetable with ID: {Id} not found", id);
                return NotFound();
            }
            return timetable;
        }


        [HttpGet("class/{id}")]
        public async Task<ActionResult<Timetable>> GetClassTimetable(int id)
        {
            _logger.LogInformation("Getting Class timetable with ID: {Id}", id);
            var timetable = await _context.Timetables.Where(tt => tt.ClassId == id).ToListAsync();
            if (timetable == null)
            {
                _logger.LogWarning("Timetable with ID: {Id} not found", id);
                return NotFound();
            }
            return Ok(timetable);
        }


        [HttpPost]
        public async Task<ActionResult<Timetable>> PostTimetable(Timetable timetable)
        {
            _logger.LogInformation("Creating new timetable");
            _context.Timetables.Add(timetable);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTimetable), new { id = timetable.TimetableId }, timetable);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTimetable(int id, Timetable timetable)
        {
            if (id != timetable.TimetableId)
            {
                return BadRequest();
            }
            _context.Entry(timetable).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTimetable(int id)
        {
            _logger.LogInformation("Deleting timetable with ID: {Id}", id);
            var timetable = await _context.Timetables.FindAsync(id);
            if (timetable == null)
            {
                return NotFound();
            }
            _context.Timetables.Remove(timetable);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
