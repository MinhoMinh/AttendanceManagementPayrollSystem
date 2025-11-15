using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.DTOs;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/leave")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly LeaveRequestService _service;

        public LeaveRequestController(LeaveRequestService service)
        {
            _service = service;
        }


        [HttpPost("request")]
        public async Task<IActionResult> Create([FromBody] LeaveRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.AddAsync(dto);
            return Ok(created);
        }

        [HttpGet("history/{empId}")]
        public async Task<IActionResult> GetHistory(int empId)
        {
            var history = await _service.GetByEmployeeIdAsync(empId);

            // ✅ Luôn trả về Ok([]) thay vì NotFound
            return Ok(history ?? Enumerable.Empty<LeaveRequestDTO>());
        }

        [HttpGet("history")]
        public async Task<IActionResult> GetLeaveHistory(
            [FromQuery] int empId,
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate)
        {
            var data = await _service.GetLeaveHistoryByEmployee(empId, startDate, endDate);
            return Ok(data);
        }

        [HttpGet("rates")]
        public ActionResult<IEnumerable<LeaveType>> GetAll()
        {
            var data = _service.GetRates();

            if (data == null) return NotFound();
            return Ok(data);
        }
    }
}


