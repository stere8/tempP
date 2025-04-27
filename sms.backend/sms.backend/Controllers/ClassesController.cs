using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using sms.backend.Services;
using sms.backend.Views;
using System.Data;

[ApiController]
[Route("api/[controller]")]
public class ClassesController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<ClassesController> _logger;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ClassesService _classesService;

    public ClassesController(UserManager<IdentityUser> userManager,
                                RoleManager<IdentityRole> roleManager,
                                SchoolContext context,
                                ILogger<ClassesController> logger, ClassesService classesService)
    {
        _context = context;
        _logger = logger;
        _classesService = classesService;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    [Authorize(Roles = "Admin,Teacher")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ClassesView>>> GetClasses([FromQuery] int? teacherId)
    {
        _logger.LogInformation("Getting classes");

        // If the user is in the Admin role, return all classes
        if (User.IsInRole("Admin"))
        {
            var classes = await _context.Classes.ToListAsync();
            List<ClassesView> classViews = new List<ClassesView>();
            foreach (var oneClass in classes)
            {
                classViews.Add(_classesService.Fill(oneClass.ClassId));
            }
            return classViews;
        }
        // If the user is in the Teacher role, return only the classes where they are enrolled
        else if (User.IsInRole("Teacher"))
        {
            // Option 1: Get teacherId from the query string
            if (!teacherId.HasValue)
            {
                return BadRequest("Teacher id is required for teacher view.");
            }

            // Get the teacher's enrollments to determine which classes to return
            var teacherEnrollments = await _context.TeacherEnrollments
                .Where(te => te.StaffId == teacherId.Value)
                .ToListAsync();

            var classIds = teacherEnrollments.Select(te => te.ClassId).Distinct().ToList();
            var classes = await _context.Classes
                .Where(c => classIds.Contains(c.ClassId))
                .ToListAsync();

            List<ClassesView> classViews = new List<ClassesView>();
            foreach (var oneClass in classes)
            {
                classViews.Add(_classesService.Fill(oneClass.ClassId));
            }
            return classViews;
        }
        else
        {
            return Forbid();
        }
    }

    [HttpPost("{classId}/message")]
    public async Task<IActionResult> SendMessageToClass(int classId, [FromBody] string messageDto)
    {
        var classItem = await _context.Classes.FindAsync(classId);
        if (classItem == null)
        {
            return NotFound("Class not found.");
        }
        // Your logic to send/store the message goes here.
        return Ok(new { success = true, message = "Message sent to class." });
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ClassesView>> GetClass(int id)
    {
        _logger.LogInformation("Getting class with ID: {Id}", id);
        var classItem = await _context.Classes.FindAsync(id);
        if (classItem == null)
        {
            _logger.LogWarning("Class with ID: {Id} not found", id);
            return NotFound();
        }
        var wegot = _classesService.Fill(classItem.ClassId);

        return wegot;
    }

    [HttpPost]
    public async Task<ActionResult<Class>> PostClass(Class? classItem)
    {
        _logger.LogInformation("Creating new class");
        _context.Classes.Add(classItem);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetClass), new { id = classItem.ClassId }, classItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutClass(int id, Class classItem)
    {
        _logger.LogInformation("Updating class with ID: {Id}", id);
        if (id != classItem.ClassId)
        {
            return BadRequest();
        }
        _context.Entry(classItem).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        _logger.LogInformation("Deleting class with ID: {Id}", id);
        var classItem = await _context.Classes.FindAsync(id);
        if (classItem == null)
        {
            return NotFound();
        }
        _context.Classes.Remove(classItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
