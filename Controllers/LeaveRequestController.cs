    using AttendanceManagementPayrollSystem.DTOs;
    using AttendanceManagementPayrollSystem.Services;
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


