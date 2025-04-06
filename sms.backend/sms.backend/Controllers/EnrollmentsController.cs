using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using sms.backend.Views;

[ApiController]
[Route("[controller]")]
public class EnrollmentsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<EnrollmentsController> _logger;

    public EnrollmentsController(SchoolContext context, ILogger<EnrollmentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EnrollmentsViews>>> GetEnrollments()
    {
        _logger.LogInformation("Getting all enrollments");
        var enrollments = await _context.Enrollments.ToListAsync();
        var students = await _context.Students.ToListAsync();
        var classes = await _context.Classes.ToListAsync();

        var returnedViewsList = new List<EnrollmentsViews>();

        foreach (var enroll in enrollments)
        {
            var student = students.FirstOrDefault(s => s.StudentId == enroll.StudentId);
            var classItem = classes.FirstOrDefault(c => c.ClassId == enroll.ClassId);

            if (student != null && classItem != null)
            {
                var studentName = $"{student.FirstName} {student.LastName}";
                returnedViewsList.Add(new EnrollmentsViews()
                {
                    EnrollmentRef = enroll.EnrollmentId,
                    EnrolledClass = classItem.Name,
                    EnrolledStudent = studentName
                });
            }
        }

        _logger.LogInformation("Successfully retrieved enrollments.");
        return Ok(returnedViewsList);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Enrollment>> GetEnrollmentById(int id)
    {
        _logger.LogInformation("Getting enrollment with ID: {Id}", id);
        var enrollment = await _context.Enrollments.FirstOrDefaultAsync(enrol => enrol.EnrollmentId == id);
        if (enrollment == null)
        {
            _logger.LogWarning("Enrollment with ID: {Id} not found", id);
            return NotFound();
        }

        return Ok(enrollment);
    }

    [HttpPost]
    public async Task<ActionResult<Enrollment>> PostEnrollment(Enrollment enrollment)
    {
        _context.Enrollments.Add(enrollment);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Enrollment created with ID: {Id}", enrollment.EnrollmentId);
        return CreatedAtAction(nameof(GetEnrollmentById), new { id = enrollment.EnrollmentId }, enrollment);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEnrollment(int id, Enrollment updatedEnrollment)
    {
        if (id != updatedEnrollment.EnrollmentId)
        {
            return BadRequest();
        }

        // Fetch the existing enrollment
        var enrollment = await _context.Enrollments.FindAsync(id);
        if (enrollment == null)
        {
            return NotFound();
        }

        // Update only non-identity fields
        enrollment.ClassId = updatedEnrollment.ClassId;
        enrollment.StudentId = updatedEnrollment.StudentId;
        // Add any other fields that need updating

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EnrollmentExists(id))
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

    private bool EnrollmentExists(int id)
    {
        return _context.Enrollments.Any(e => e.EnrollmentId == id);
    }


    [HttpDelete("{enrollmentId}")]
    public async Task<IActionResult> DeleteEnrollment(int enrollmentId)
    {
        _logger.LogInformation("Deleting enrollment with ID: {EnrollmentId}", enrollmentId);
        var enrollment = await _context.Enrollments.FirstOrDefaultAsync(enr => enr.EnrollmentId == enrollmentId);
        if (enrollment == null)
        {
            _logger.LogWarning("Enrollment with ID: {EnrollmentId} not found", enrollmentId);
            return NotFound();
        }

        _context.Enrollments.Remove(enrollment);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Enrollment with ID: {EnrollmentId} deleted successfully", enrollmentId);
        return NoContent();
    }
}