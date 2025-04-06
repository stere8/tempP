using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using System.Linq;
using System.Threading.Tasks;
using sms.backend.Models;
using Microsoft.AspNetCore.Identity;
using sms.backend.Services;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly SchoolContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ClassesService _classesService;


        public DashboardController(SchoolContext context, UserManager<IdentityUser> userManager, ClassesService classesService)
        {
            _context = context;
            _userManager = userManager;
            _classesService = classesService;
        }

        // GET: api/dashboard/admin
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminDashboard()
        {
            var totalStudents = await _context.Students.CountAsync();
            var totalTeachers = await _context.Staff.CountAsync();
            var totalParents = await _context.StudentParents
                .Select(sp => sp.ParentId)
                .Distinct()
                .CountAsync();

            return Ok(new { totalStudents, totalTeachers, totalParents });
        }

        // GET: api/dashboard/parent/{parentId}
        [Authorize(Roles = "Parent")]
        [HttpGet("parent/{parentId}")]
        public async Task<IActionResult> GetParentDashboard(string parentId)
        {
            var parent = await _context.Parents
                        .Include(p => p.StudentParents)
                                    .ThenInclude(sp => sp.Student)
                                    .FirstOrDefaultAsync(p => p.UserId == parentId);

            if (parent == null)
            {
                return NotFound("Parent not found.");
            }

            var students = parent.StudentParents.Select(sp => sp.Student);
            return Ok(students);
        }

        [Authorize(Roles = "Teacher")]
        [HttpGet("teacher/{teacherId}")]
        public async Task<IActionResult> GetTeacherDashboard(string teacherId)
        {
            if (string.IsNullOrWhiteSpace(teacherId))
            {
                return BadRequest("Invalid teacher ID");
            }

            var teacher = await _context.Staff.FirstOrDefaultAsync(s => s.UserId == teacherId);

            if (teacher == null)
            {
                return NotFound("Teacher not found.");
            }

            var teacherEnrollments = await _context.TeacherEnrollments
                .Where(te => te.StaffId == teacher.StaffId)
                .ToListAsync();

            var response = new List<object>();
            var subjectsList = new List<Lesson>();
            var classesList = new List<Class>();

            foreach (var enrollment in teacherEnrollments)
            {
               subjectsList.Add(_context.Lessons.FirstOrDefault(e => e.LessonId == enrollment.LessonId));
               classesList.Add(_context.Classes.FirstOrDefault(e => e.ClassId == enrollment.ClassId));
            }


            foreach (var enrollment in teacherEnrollments)
            {
                response.Add(new
                {
                    Classes = classesList,
                    Subject = subjectsList,
                    Schedule = _classesService.GetTeacherTimetable(teacher.StaffId),
                });
            }

            return Ok(response);
        }

        // GET: api/dashboard/student/{studentId}
        [Authorize(Roles = "Student")]
        [HttpGet("student/{userId}")]
        public async Task<IActionResult> GetStudentDashboard(string userId)
        {

            var student = _context.Students.FirstOrDefault(s => s.UserId == userId);
            if (!string.IsNullOrEmpty(student.UserId))
            {
                student.User = await _userManager.FindByIdAsync(student.UserId);
            }
            int studentId = student.StudentId;
            var enrollment = await _context.Enrollments
                .FirstOrDefaultAsync(e => e.StudentId == studentId);

            if (enrollment == null)
            {
                return NotFound("Student enrollment not found.");
            }

            var classData = _classesService.Fill(enrollment.ClassId);


            var attendanceSummary = await _context.Attendances
                .Where(a => a.StudentId == student.StudentId)
                .GroupBy(a => a.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            var marks = await _context.Marks
                .Where(m => m.StudentId == studentId)
                .ToListAsync();

            return Ok(new { classData, attendanceSummary, marks });
        }
    }
}
