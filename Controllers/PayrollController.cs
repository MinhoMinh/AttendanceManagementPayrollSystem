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

        [HttpPost("approve/first/{id}")]
        public async Task<IActionResult> ApproveFirst(int id, [FromBody] ApproveRequest request)
        {
            var result = await _service.ApproveFirstAsync(id, request.ApprovedBy);
            return Ok(result);
        }

        [HttpPost("approve/final/{id}")]
        public async Task<IActionResult> ApproveFinal(int id, [FromBody] ApproveRequest request)
        {
            var result = await _service.ApproveFinalAsync(id, request.ApprovedBy);
            return Ok(result);
        }

        [HttpPost("reject/{id}")]
        public async Task<IActionResult> Reject(int id, [FromBody] ApproveRequest request)
        {
            var result = await _service.RejectAsync(id, request.ApprovedBy);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

    }
}
