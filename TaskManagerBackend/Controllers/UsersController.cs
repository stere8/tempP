using Microsoft.AspNetCore.Mvc;
using TaskManagerBackend.Models;
using System.Collections.Generic;

namespace TaskManagerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly List<User> _users;

        // DI will provide the same List<User> registered in Program.cs
        public UsersController(List<User> users)
        {
            _users = users;
        }

        // GET /api/users
        [HttpGet]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_users);
        }
    }
}
