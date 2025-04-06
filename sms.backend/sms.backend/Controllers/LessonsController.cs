using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

[ApiController]
[Route("[controller]")]
public class LessonsController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<LessonsController> _logger;

    public LessonsController(SchoolContext context, ILogger<LessonsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Lesson>>> GetLessons()
    {
        _logger.LogInformation("Getting all lessons");
        return await _context.Lessons.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Lesson>> GetLesson(int id)
    {
        _logger.LogInformation("Getting lesson with ID: {Id}", id);
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null)
        {
            _logger.LogWarning("Lesson with ID: {Id} not found", id);
            return NotFound();
        }
        return lesson;
    }

    [HttpPost]
    public async Task<ActionResult<Lesson>> PostLesson(Lesson lesson)
    {
        _logger.LogInformation("Creating new lesson");
        _context.Lessons.Add(lesson);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetLesson), new { id = lesson.LessonId }, lesson);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutLesson(int id, Lesson lesson)
    {
        if (id != lesson.LessonId)
        {
            return BadRequest();
        }
        _context.Entry(lesson).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLesson(int id)
    {
        _logger.LogInformation("Deleting lesson with ID: {Id}", id);
        var lesson = await _context.Lessons.FindAsync(id);
        if (lesson == null)
        {
            return NotFound();
        }
        _context.Lessons.Remove(lesson);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("grade/{gradeLevel}")]
    public async Task<ActionResult<IEnumerable<Lesson>>> GetLessonsByGradeLevel(int gradeLevel)
    {
        _logger.LogInformation("Getting lessons for grade level: {GradeLevel}", gradeLevel);
        var lessons = await _context.Lessons.Where(l => l.GradeLevel == gradeLevel).ToListAsync();
        return Ok(lessons);
    }
}
