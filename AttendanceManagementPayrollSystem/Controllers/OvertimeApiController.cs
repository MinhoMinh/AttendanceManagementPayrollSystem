using Microsoft.AspNetCore.Mvc;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.Interfaces;
using System;

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

        // ✅ GET /api/overtime/my
        [HttpGet("my")]
        public IActionResult GetMyOvertimeHistory()
        {
            int currentEmpId = GetCurrentEmployeeId();

            var data = _service.GetOvertimeHistoryByEmployee(currentEmpId);

            return Ok(data);
        }


        // 🆕 POST /api/overtime/create
        [HttpPost("create")]
        public IActionResult CreateOvertime([FromBody] OvertimeRequest request)
        {
            if (request == null) return BadRequest("Invalid request data.");

            request.EmpId = GetCurrentEmployeeId();
            request.Status = "Pending";
            request.CreatedDate = DateOnly.FromDateTime(DateTime.Now);

            _service.CreateOvertimeRequest(request);
            return Ok(new { message = "Overtime request submitted successfully." });
        }
        // ✅ PUT /api/overtime/approve/{id}
        [HttpPut("approve/{id}")]
        public IActionResult ApproveOvertime(int id)
        {
            int approverId = GetCurrentEmployeeId(); // giả lập người duyệt
            _service.ApproveOvertimeRequest(id, approverId);
            return Ok(new { message = "Overtime request approved." });
        }


        // ❌ PUT /api/overtime/reject/{id}
        [HttpPut("reject/{id}")]
        public IActionResult RejectOvertime(int id)
        {
            int approverId = GetCurrentEmployeeId();
            _service.RejectOvertimeRequest(id, approverId);
            return Ok(new { message = "Overtime request rejected." });
        }

        private int GetCurrentEmployeeId()
        {
            // 🔒 Tạm thời hardcode, sau này thay bằng session/login user context
            return 1;
        }
    }
}
