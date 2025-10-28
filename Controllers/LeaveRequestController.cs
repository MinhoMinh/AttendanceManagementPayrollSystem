﻿    using AttendanceManagementPayrollSystem.DTOs;
    using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LeaveRequestController : ControllerBase
    {
        private readonly LeaveRequestService _service;

        public LeaveRequestController(LeaveRequestService service)
        {
            _service = service;
        }

        [HttpGet("pending")]
        public async Task<IActionResult> GetPending()
        {
            var list = await _service.GetPendingAsync();
            return Ok(list);
        }

        [HttpPut("update-status")]
        public async Task<IActionResult> UpdateStatus([FromBody] LeaveRequestStatusDTO dto)
        {

            Console.WriteLine($"Incoming: ReqId={dto.ReqId}, Status={dto.Status}");
            await _service.UpdateStatusAsync(dto);
            return NoContent();
        }

        // shortcut endpoints
        [HttpPut("{id}/approve")]
        public async Task<IActionResult> Approve(int id)
        {
            await _service.UpdateStatusAsync(new LeaveRequestStatusDTO { ReqId = id, Status = "Approved" });
            return NoContent();
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> Reject(int id)
        {
            await _service.UpdateStatusAsync(new LeaveRequestStatusDTO { ReqId = id, Status = "Rejected" });
            return NoContent();
        }

        [HttpPost]
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
    }
}


