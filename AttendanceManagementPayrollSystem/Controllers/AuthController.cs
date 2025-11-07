using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest(new { message = "Vui lòng nhập đủ Username và Password." });

            var response = await _authService.LoginAsync(request.Username, request.Password);

            if (response == null)
                return Unauthorized(new { message = "Sai tài khoản hoặc mật khẩu." });

            return Ok(response);
        }
    }
}
