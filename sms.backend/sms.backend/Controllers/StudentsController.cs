using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly ILogger<StudentsController> _logger;
        private readonly UserManager<IdentityUser> _userManager;

        public StudentsController(SchoolContext context, ILogger<StudentsController> logger, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            _logger.LogInformation("Getting all students");
            var student =  await _context.Students.ToListAsync();

            return student;
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            _logger.LogInformation("Getting student with ID: {Id}", id);
            var student = await _context.Students.FindAsync(id);
            if (!string.IsNullOrEmpty(student.UserId))
            {
                student.User = await _userManager.FindByIdAsync(student.UserId);
            }
            if (student == null)
            {
                _logger.LogWarning("Student with ID: {Id} not found", id);
                return NotFound();
            }

            student.User = await _userManager.FindByIdAsync(student.UserId);
            return student;
        }

        // POST: api/Students
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            _logger.LogInformation("Creating new student");
            _context.Students.Add(student);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error occurred while creating student");
                return StatusCode(500, "An error occurred while creating the student. Please try again.");
            }

            return CreatedAtAction(nameof(GetStudent), new { id = student.StudentId }, student);
        }

        // PUT: api/Students/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.StudentId)
            {
                _logger.LogWarning("Mismatched ID for student update");
                return BadRequest("The provided ID does not match the student's ID.");
            }

            if (string.IsNullOrEmpty(student.UserId))
            {
                student.User = null;
                student.UserId = null;
            }
            else { }

                _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!StudentExists(id))
                {
                    _logger.LogWarning("Student with ID: {Id} not found for update", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Error occurred while updating student");
                    return StatusCode(500, "An error occurred while updating the student. Please try again.");
                }
            }

            return NoContent();
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                _logger.LogWarning("Student with ID: {Id} not found for deletion", id);
                return NotFound();
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Student with ID: {Id} deleted", id);
            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.StudentId == id);
        }
    }
}