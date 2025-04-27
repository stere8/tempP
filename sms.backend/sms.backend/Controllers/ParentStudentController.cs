using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ParentStudentController : Controller
    {
        private readonly SchoolContext _context;

        public ParentStudentController(SchoolContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAssignments()
        {
            var assignments = await _context.StudentParents
                .Include(sp => sp.Parent)
                .Include(sp => sp.Student)
                .ToListAsync();
            return Ok(assignments);
        }

        [HttpGet("parent/{parentId}")]
        public async Task<IActionResult> GetAssignmentsForParent(int parentId)
        {
            var parent = _context.Parents.FirstOrDefault(s => s.ParentId == parentId);
            var assignments = await _context.StudentParents
                .Where(sp => sp.ParentId.Equals(parent.UserId))
                .Include(sp => sp.Student)
                .ToListAsync();
            if (!assignments.Any())
            {
                return NotFound("No student assignments found for this parent.");
            }
            return Ok(assignments);
        }

        // POST: api/ParentStudent/assign
        [HttpPost("assign")]
        public async Task<IActionResult> AssignStudentToParent([FromBody] ParentStudentAssignmentDto dto)
        {
            // Validate the parent exists.
            var parent = await _context.Parents.FindAsync(dto.ParentId);
            if (parent == null)
                return NotFound("Parent not found.");

            // Validate the student exists.
            var student = await _context.Students.FindAsync(dto.StudentId);
            if (student == null)
                return NotFound("Student not found.");

            // Check if the assignment already exists.
            bool alreadyAssigned = await _context.StudentParents
                .AnyAsync(sp => sp.ParentId.Equals(parent.UserId) && sp.StudentId == dto.StudentId);
            if (alreadyAssigned)
            {
                return BadRequest("The student is already assigned to this parent.");
            }

            // Create a new assignment.
            var assignment = new StudentParent
            {
                ParentId = dto.ParentId,
                StudentId = dto.StudentId,
                IsPrimary = dto.IsPrimary
            };

            _context.StudentParents.Add(assignment);
            await _context.SaveChangesAsync();

            return Ok(assignment);
        }
    }
}
