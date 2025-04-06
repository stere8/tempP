using Microsoft.AspNetCore.Mvc;
using sms.backend.Data;
using sms.backend.Models;

namespace sms.backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : Controller
    {
        SchoolContext _context;
        public MessageController(SchoolContext context) 
        {
            _context = context;
        }
        // POST: api/message
        [HttpPost]
        public IActionResult AddMessage([FromBody] Message message)
        {
            if (message == null || string.IsNullOrEmpty(message.Content))
            {
                return BadRequest("Invalid message data.");
            }

            message.CreatedAt = DateTime.UtcNow;
            _context.Messages.Add(message);

            return Ok(message);
        }

        // GET: api/message?targetType=Student&targetId=123
        [HttpGet]
        public IActionResult GetMessages([FromQuery] MessageTargetType? targetType, [FromQuery] int? targetId)
        {
            IEnumerable<Message> result = _context.Messages.ToList();

            if (targetType.HasValue)
            {
                result = result.Where(m => m.TargetType == targetType.Value);
            }

            if (targetId.HasValue)
            {
                result = result.Where(m => m.TargetId == targetId.Value);
            }

            // Filter messages older than 24 hours (if needed)
            result = result.Where(m => (DateTime.UtcNow - m.CreatedAt).TotalHours <= 24);

            return Ok(result);
        }

        // GET: api/message/all
        // Admin-only: Retrieve all messages regardless of expiry.
        [HttpGet("all")]
        public IActionResult GetAllMessages()
        {
            return Ok(_context.Messages.ToList());
        }

        // GET: api/message/recipients?role=Teacher&userId=2&classId=101
        [HttpGet("recipients")]
        public IActionResult GetRecipients([FromQuery] string role, [FromQuery] int? userId, [FromQuery] int? classId)
        {
            // Dummy data for demonstration purposes.
            var allStudents = new List<Recipient>
            {
                new Recipient { Type = "Student", Id = 1, Name = "Alice Johnson" },
                new Recipient { Type = "Student", Id = 2, Name = "Bob Smith" },
                new Recipient { Type = "Student", Id = 3, Name = "Charlie Davis" }
            };

            var allClasses = new List<Recipient>
            {
                new Recipient { Type = "Class", Id = 101, Name = "Class A" },
                new Recipient { Type = "Class", Id = 102, Name = "Class B" }
            };

            List<Recipient> result = new List<Recipient>();

            if (role.Equals("Parent", StringComparison.OrdinalIgnoreCase))
            {
                // For Parents: Return only the students assigned to them.
                // Replace with your logic to filter based on parent's userId.
                // For demo, assume parent's assigned students are with IDs 1 and 2.
                result = allStudents.Where(s => s.Id == 1 || s.Id == 2).ToList();
            }
            else if (role.Equals("Teacher", StringComparison.OrdinalIgnoreCase))
            {
                // For Teachers: Return students assigned to your class and the class option.
                if (!classId.HasValue)
                {
                    return BadRequest("classId is required for teacher role.");
                }
                // Replace with your logic to fetch students based on teacher's class.
                // For demo, assume teacher's class has students with IDs 2 and 3.
                result = allStudents.Where(s => s.Id == 2 || s.Id == 3).ToList();
                // Also include the class itself.
                var teacherClass = allClasses.FirstOrDefault(c => c.Id == classId.Value);
                if (teacherClass != null)
                {
                    result.Add(teacherClass);
                }
            }
            else if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
            {
                // For Admin: Get all students and classes.
                result.AddRange(allStudents);
                result.AddRange(allClasses);
            }
            else
            {
                return BadRequest("Invalid role specified.");
            }

            return Ok(result);
        }
    }
}
