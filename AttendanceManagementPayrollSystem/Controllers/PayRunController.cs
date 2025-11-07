using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Azure.Core;
using DocumentFormat.OpenXml.Office2016.Excel;
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
        public async Task<ActionResult<PayRunDto>> GenerateRegularPayRun([FromBody] PayRunRequest request)
        {
            if (request == null || request.PeriodMonth < 1 || request.PeriodMonth > 12)
                return BadRequest("Invalid period.");

            var exists = await _service.ContainsValidPayRunInPeriod(request.PeriodMonth, request.PeriodYear);
            if (exists)
                return Conflict("Pay run for this period already exists.");

            try {
                var result = await _service.GenerateRegularPayRun(request.Name, request.PeriodMonth, request.PeriodYear, request.CreatedBy);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            return NotFound();
        }

        [HttpPost("save")]
        public async Task<IActionResult> SaveRegularPayRun([FromBody] PayRunDto payRunDto)
        {
            Console.Write("hell?");
            if (payRunDto == null)
                return BadRequest("Invalid pay run data.");
            try
            {
                await _service.SaveRegularPayRun(payRunDto);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
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

        [HttpPost("approve-first")]
        public async Task<IActionResult> ApproveFirstPayRun([FromBody] ApproveRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            try
            {
                await _service.ApproveFirst(request.ApproverId, request.PayRunId);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("approve-final")]
        public async Task<IActionResult> ApproveFinalPayRun([FromBody] ApproveRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            try
            {
                await _service.ApproveFinal(request.ApproverId, request.PayRunId);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("reject")]
        public async Task<IActionResult> RejectPayRun([FromBody] ApproveRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            try
            {
                await _service.Reject(request.ApproverId, request.PayRunId);

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("getpayrun/{empId}/{periodMonth}/{periodYear}")]
        public async Task<ActionResult<List<PayRun>>> GetPayRunByEmpIdAndDate(int empId, int periodMonth, int periodYear)
        {
            var result = await _service.GetPayRunByEmpIdAndDateAsync(empId, periodMonth, periodYear);
            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
