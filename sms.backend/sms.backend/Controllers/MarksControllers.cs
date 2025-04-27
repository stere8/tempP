using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

[ApiController]
[Route("api/[controller]")]
public class MarksController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<MarksController> _logger;

    public MarksController(SchoolContext context, ILogger<MarksController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mark>>> GetMarks()
    {
        _logger.LogInformation("Getting all marks");
        return await _context.Marks.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Mark>> GetMark(int id)
    {
        _logger.LogInformation("Getting mark with ID: {Id}", id);
        var mark = await _context.Marks.FindAsync(id);
        if (mark == null)
        {
            _logger.LogWarning("Mark with ID: {Id} not found", id);
            return NotFound();
        }
        return mark;
    }


    [Authorize(Roles = "Admin")]
    [HttpGet("report/{studentID}")]
    public async Task<IActionResult> GenerateStudentReport(int studentID)
    {
        // Get all marks for the student asynchronously
        var allGotenMarks = await _context.Marks
            .Where(m => m.StudentId == studentID)
            .ToListAsync();

        // Group marks by lesson using a Dictionary<Lesson, List<Mark>>
        Dictionary<Lesson, List<Mark>> marksByLesson = new Dictionary<Lesson, List<Mark>>();

        foreach (var mark in allGotenMarks)
        {
            // Retrieve the lesson for the current mark asynchronously
            var lesson = await _context.Lessons.FirstOrDefaultAsync(l => l.LessonId == mark.LessonId);
            if (lesson == null)
            {
                // If lesson not found, skip this mark
                continue;
            }

            // Option 1: Use the Lesson object as a key (requires proper Equals/GetHashCode in Lesson)
            if (marksByLesson.ContainsKey(lesson))
            {
                marksByLesson[lesson].Add(mark);
            }
            else
            {
                marksByLesson[lesson] = new List<Mark> { mark };
            }
        }

        // Get student details
        var student = await _context.Students.FindAsync(studentID);
        if (student == null)
        {
            return NotFound("Student not found.");
        }

        // Build the report object, calculating the average mark for each lesson.
        // Here we assume each Mark has a numeric property 'MarkValue' on a scale of 0-100.
        // Dividing by 10 converts it to a 0-10 scale.
        var report = new
        {
            Student = student,
            MarksByLesson = marksByLesson.Select(kvp => new
            {
                Lesson = kvp.Key,
                Marks = kvp.Value,
                AverageMark = Math.Round(kvp.Value.Average(m => m.MarkValue) / 10, 1)
            })
        };

        return Ok(report);
    }



    [HttpPost]
    public async Task<ActionResult<Mark>> PostMark(Mark mark)
    {
        _logger.LogInformation("Creating new mark");
        _context.Marks.Add(mark);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMark), new { id = mark.MarkId }, mark);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutMark(int id, Mark mark)
    {
        if (id != mark.MarkId)
        {
            return BadRequest();
        }
        _context.Entry(mark).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMark(int id)
    {
        _logger.LogInformation("Deleting mark with ID: {Id}", id);
        var mark = await _context.Marks.FindAsync(id);
        if (mark == null)
        {
            return NotFound();
        }
        _context.Marks.Remove(mark);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
