using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

[ApiController]
[Route("api/[controller]")]
public class StaffController : ControllerBase
{
    private readonly SchoolContext _context;
    private readonly ILogger<StaffController> _logger;

    public StaffController(SchoolContext context, ILogger<StaffController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: /staff
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Staff>>> GetStaff()
    {
        _logger.LogInformation("Getting all staff members");
        var gottenStaff = await _context.Staff.ToListAsync();
        return Ok(gottenStaff);
    }

    // GET: /staff/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Staff>> GetStaff(int id)
    {
        _logger.LogInformation("Getting staff member with ID: {Id}", id);
        var staff = await _context.Staff.FindAsync(id);
        if (staff == null)
        {
            _logger.LogWarning("Staff member with ID: {Id} not found", id);
            return NotFound();
        }
        return staff;
    }

    // POST: /staff
    [HttpPost]
    public async Task<ActionResult<Staff>> PostStaff(Staff staff)
    {
        _logger.LogInformation("Creating new staff member");
        _context.Staff.Add(staff);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStaff), new { id = staff.StaffId }, staff);
    }

    // PUT: /staff/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutStaff(int id, Staff staff)
    {
        _logger.LogInformation("Updating staff member with ID: {Id}", id);
        if (id != staff.StaffId)
        {
            return BadRequest();
        }
        _context.Entry(staff).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    // DELETE: /staff/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        _logger.LogInformation("Deleting staff member with ID: {Id}", id);
        var staff = await _context.Staff.FindAsync(id);
        if (staff == null)
        {
            return NotFound();
        }
        _context.Staff.Remove(staff);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
