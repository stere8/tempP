using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;

[ApiController]
[Route("[controller]")]
public class StaffController(SchoolContext context, ILogger<StaffController> logger) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Staff>>> GetStaff()
    {
        logger.LogInformation("Getting all staff members");
        return await context.Staff.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Staff>> GetStaff(int id)
    {
        logger.LogInformation("Getting staff member with ID: {Id}", id);
        var staff = await context.Staff.FindAsync(id);
        if (staff == null)
        {
            logger.LogWarning("Staff member with ID: {Id} not found", id);
            return NotFound();
        }
        return staff;
    }

    [HttpPost]
    public async Task<ActionResult<Staff>> PostStaff(Staff? staff)
    {
        logger.LogInformation("Creating new staff member");
        context.Staff.Add(staff);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStaff), new { id = staff.StaffId }, staff);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutStaff(int id, Staff staff)
    {
        logger.LogInformation("Updating staff member with ID: {Id}", id);
        if (id != staff.StaffId)
        {
            return BadRequest();
        }
        context.Entry(staff).State = EntityState.Modified;
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteStaff(int id)
    {
        logger.LogInformation("Deleting staff member with ID: {Id}", id);
        var staff = await context.Staff.FindAsync(id);
        if (staff == null)
        {
            return NotFound();
        }
        context.Staff.Remove(staff);
        await context.SaveChangesAsync();
        return NoContent();
    }
}
