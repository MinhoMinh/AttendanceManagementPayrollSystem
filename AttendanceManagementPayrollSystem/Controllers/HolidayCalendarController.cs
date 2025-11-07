using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HolidayCalendarController : ControllerBase
    {
        private readonly HolidayCalendarService _service;

        public HolidayCalendarController(HolidayCalendarService service)
        {
            _service = service;
        }


        // GET: /api/holidays?start=2025-01-01&end=2025-12-31&name=New
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HolidayCalendarDTO>>> GetHolidays(
            [FromQuery] DateTime? start,
            [FromQuery] DateTime? end,
            [FromQuery] string? name)
        {
            var holidays = await _service.GetFilteredHolidaysAsync(start, end, name);
            return Ok(holidays);
        }

        /// <summary>
        /// Lấy thông tin chi tiết ngày nghỉ lễ theo ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var holiday = await _service.GetByIdAsync(id);
            if (holiday == null)
                return NotFound(new { message = "Holiday not found." });

            return Ok(holiday);
        }

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ áp dụng cho một phòng ban cụ thể.
        /// </summary>
        [HttpGet("department/{depId}")]
        public async Task<IActionResult> GetByDepartment(int depId)
        {
            var result = await _service.GetByDepartmentAsync(depId);
            return Ok(result);
        }

        [HttpGet("employee/{empId}/range")]
        public async Task<IActionResult> GetByEmployeeInRange(
    int empId,
    [FromQuery] DateTime start,
    [FromQuery] DateTime end)
        {
            if (start > end) return BadRequest("Start must be before or equal to End.");

            var result = await _service.GetByEmployeeInRange(empId, start, end);
            return Ok(result);
        }

        /// <summary>
        /// Tạo mới một ngày nghỉ lễ.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] HolidayCalendarDTO dto)
        {
            if (dto == null)
                return BadRequest(new { message = "Invalid data." });

            var created = await _service.AddAsync(dto);
            return Ok(created);
        }

        /// <summary>
        /// Cập nhật thông tin ngày nghỉ lễ.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HolidayCalendarDTO dto)
        {
            if (dto == null || dto.HolidayId != id)
                return BadRequest(new { message = "Invalid data or mismatched ID." });

            var updated = await _service.UpdateAsync(dto);
            return Ok(updated);
        }

        /// <summary>
        /// Gán ngày nghỉ lễ cho một phòng ban.
        /// </summary>
        [HttpPost("{holidayId}/assign/{depId}")]
        public async Task<IActionResult> AssignToDepartment(int holidayId, int depId, DateTime startDate, DateTime endDate)
        {
            await _service.AssignHolidayToDepartmentAsync(holidayId, depId, startDate, endDate);
            return Ok(new { message = "Assigned successfully." });
        }

        /// <summary>
        /// Gỡ ngày nghỉ lễ khỏi một phòng ban.
        /// </summary>
        [HttpDelete("{holidayId}/remove/{depId}")]
        public async Task<IActionResult> RemoveFromDepartment(int holidayId, int depId)
        {
            await _service.RemoveHolidayFromDepartmentAsync(holidayId, depId);
            return Ok(new { message = "Removed successfully." });
        }
    }
}
