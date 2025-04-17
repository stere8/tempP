using Microsoft.AspNetCore.Mvc;
using TaskManagerBackend.Models;
using TaskManagerBackend.Services;

namespace TaskManagerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskServices _taskServices;
        public TasksController(ITaskServices taskServices) 
        {
           _taskServices = taskServices;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllTasks(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
        {
            // Validate input
            page = page < 1 ? 1 : page;
            pageSize = pageSize switch
            {
                > 100 => 100,
                < 1 => 10,
                _ => pageSize
            };

            var allTasks = await _taskServices.GetAllTasks();
            var totalCount = allTasks.Count;

            var sortedTasks = allTasks
                .OrderByDescending(t => t.Difficulty)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new PaginatedResponse<TaskBase>
            {
                Data = sortedTasks,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            });
        }

        [HttpPost("assign")]
        public IActionResult AssignTasks([FromBody] AssignmentRequest request)
        {
            var result = _taskServices.AssignTasks(request);
            if (result.IsSuccess)
            {
                return Ok(result.Message);
            }
            return BadRequest(result.Message);
        }


        [HttpGet("user/{id}")]
        public IActionResult GetTasksByUserId(int id)
        {
            var tasks = _taskServices.GetTasksByUserIdAsync(id).Result;
            if (tasks == null || tasks.Count == 0)
            {
                return NotFound("No tasks found for this user.");
            }
            return Ok(tasks);
        }
    }
}
