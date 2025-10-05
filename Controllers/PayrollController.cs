using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PayrollController : ControllerBase
    {
        private readonly PayrollService _service;

        public PayrollController(PayrollService service)
        {
            _service = service;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> Generate([FromBody] PayrollRequest request)
        {
            var result = await _service.GeneratePayrollAsync(request.Name, request.PeriodMonth, request.PeriodYear, request.CreatedBy);
            return Ok(result);
        }
    }
}
