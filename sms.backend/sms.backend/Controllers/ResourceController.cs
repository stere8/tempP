using Microsoft.AspNetCore.Mvc;
using sms.backend.Data;
using sms.backend.Models;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResourceController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly IWebHostEnvironment _environment;

        public ResourceController(SchoolContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // POST: api/resource/upload
        [Authorize(Roles = "Teacher,Admin")]
        [HttpPost("upload")]
        public async Task<IActionResult> UploadResource([FromForm] ResourceUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Ensure uploads folder exists (wwwroot/uploads)
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate a unique file name
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(dto.File.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // Create new Resource record
            var resource = new Resource
            {
                Title = dto.Title,
                Description = dto.Description,
                FilePath = "/uploads/" + fileName, // relative URL
                FileType = Path.GetExtension(dto.File.FileName).TrimStart('.'),
                ClassId = dto.ClassId,
                UploadedBy = dto.UploadedBy, // can be 0 or placeholder for now
                UploadDate = DateTime.UtcNow
            };

            _context.Resources.Add(resource);
            await _context.SaveChangesAsync();

            return Ok(resource);
        }

        // GET: api/resource/class/{classId}
        [HttpGet("class/{classId}")]
        public async Task<IActionResult> GetResourcesByClass(int classId)
        {
            var resources = await _context.Resources
                .Where(r => r.ClassId == classId)
                .ToListAsync();

            return Ok(resources);
        }

        // PUT: api/resource/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResource(int id, [FromBody] ResourceUpdateDto dto)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            resource.Title = dto.Title;
            resource.Description = dto.Description;
            await _context.SaveChangesAsync();

            return Ok(resource);
        }

        // DELETE: api/resource/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResource(int id)
        {
            var resource = await _context.Resources.FindAsync(id);
            if (resource == null)
            {
                return NotFound();
            }

            // Optionally remove file from disk
            var fullPath = Path.Combine(_environment.WebRootPath, resource.FilePath.TrimStart('/'));
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            _context.Resources.Remove(resource);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
