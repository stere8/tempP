using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

[ApiController]
[Route("[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<AttendanceController> _logger;

    public AttendanceController(SchoolContext context, ILogger<AttendanceController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Attendance>>> sGetAttendance()
    {
        _logger.LogInformation("Getting all attendance records");
        return await _context.Attendances.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Attendance>> GetAttendanceRecord(int id)
    {
        _logger.LogInformation("Getting attendance record with ID: {Id}", id);
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null)
        {
            _logger.LogWarning("Attendance record with ID: {Id} not found", id);
            return NotFound();
        }
        return attendance;
    }

    [HttpPost]
    public async Task<ActionResult<Attendance>> PostAttendance(Attendance attendance)
    {
        _logger.LogInformation("Creating new attendance record");
        _context.Attendances.Add(attendance);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAttendanceRecord), new { id = attendance.AttendanceId }, attendance);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutAttendance(int id, Attendance attendance)
    {
        if (id != attendance.AttendanceId)
        {
            return BadRequest();
        }
        _context.Entry(attendance).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAttendance(int id)
    {
        _logger.LogInformation("Deleting attendance record with ID: {Id}", id);
        var attendance = await _context.Attendances.FindAsync(id);
        if (attendance == null)
        {
            return NotFound();
        }
        _context.Attendances.Remove(attendance);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
