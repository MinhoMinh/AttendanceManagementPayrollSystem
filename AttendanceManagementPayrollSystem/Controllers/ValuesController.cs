using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHello()
        {
            return Ok(new { message = "Hello from .NET API (proper controller)" });
        }

        [HttpGet("greet/{name}")]
        public IActionResult Greet(string name)
        {
            return Ok(new { message = $"Hello, {name}!" });
        }
    }
}
