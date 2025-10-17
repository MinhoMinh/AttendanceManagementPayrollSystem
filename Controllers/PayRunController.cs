using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/pay-run")]
    public class PayRunController : ControllerBase
    {
        private readonly PayRunService _service;

        public PayRunController(PayRunService service)
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
        public async Task<ActionResult<List<PayRunBasicDto>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PayRunDto>> GetPayRun(int id)
        {
            var result = await _service.GetPayRunAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

    }
}
