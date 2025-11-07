using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/overtime")]
    public class OvertimeApiController : ControllerBase
    {
        private readonly IOvertimeService _service;

        public OvertimeApiController(IOvertimeService service)
        {
            _service = service;
        }

        // ✅ GET /api/overtime/history?empId=1&startDate=2025-10-01&endDate=2025-10-30
        [HttpGet("history")]
        public async Task<IActionResult> GetOvertimeHistory(
            [FromQuery] int empId,
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate)
        {
            var data = await _service.GetOvertimeHistoryByEmployee(empId, startDate, endDate);
            return Ok(data);
        }

        // ✅ POST /api/overtime/request (Employee self-request)
        [HttpPost("request")]
        public IActionResult RequestOvertime([FromBody] OvertimeRequest request)
        {
            if (request == null) return BadRequest("Invalid request data.");

            request.Status = "Pending";
            request.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

            _service.CreateOvertimeRequest(request);
            return Ok(new { message = "Overtime request submitted successfully." });
        }

        // ✅ POST /api/overtime/request-by-head (Department head creates on behalf)
        [HttpPost("request-by-head")]
        public IActionResult RequestOvertimeByHead([FromBody] OvertimeRequest request)
        {
            if (request == null) return BadRequest("Invalid request data.");

            request.Status = "Approved";
            request.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

            _service.CreateOvertimeRequestByHead(request);
            return Ok(new { message = "Overtime request submitted by department head." });
        }

        // ✅ PUT /api/overtime/approve/{id}
        [HttpPut("approve/{id}")]
        public IActionResult ApproveOvertime(int id, [FromBody] ApproveRejectDto dto)
        {
            _service.ApproveOvertimeRequest(id, dto.ApproverId);
            return Ok(new { message = "Overtime request approved." });
        }

        // ✅ PUT /api/overtime/reject/{id}
        [HttpPut("reject/{id}")]
        public IActionResult RejectOvertime(int id, [FromBody] ApproveRejectDto dto)
        {
            _service.RejectOvertimeRequest(id, dto.ApproverId);
            return Ok(new { message = "Overtime request rejected." });
        }

        //[HttpPost("headrequest")]
        //public async ActionResult<IEnumerable<OvertimeRequestDTO>> GetRequestOvertimesByHead([FromQuery] int headId,
        //    [FromQuery] DateOnly? startDate,
        //    [FromQuery] DateOnly? endDate)
        //{
        //    var data = await _service.GetOvertimeHistoryByHead(headId, startDate, endDate);
        //    return Ok(data);
        //}


        [HttpGet("rates")]
        public ActionResult<IEnumerable<OvertimeRateDTO>> GetAll()
        {
            var data = _service.GetRates();

            if (data == null) return NotFound();
            return Ok(data);
        }

    }

    // DTOs
    public class ApproveRejectDto
    {
        public int ApproverId { get; set; }
    }
}
