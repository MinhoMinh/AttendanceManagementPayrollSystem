using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClockinController : ControllerBase
    {
        private readonly ClockinService _clockinService;

        public ClockinController(ClockinService clockinService)
        {
            _clockinService = clockinService;
        }

        /// <summary>
        /// Lấy toàn bộ dữ liệu chấm công của 1 nhân viên trong 1 tháng
        /// </summary>
        /// <param name="empId">Mã nhân viên</param>
        /// <param name="month">Tháng cần xem</param>
        /// <param name="year">Năm cần xem</param>
        [HttpGet("employee/{empId}")]
        public async Task<IActionResult> GetClockinsByEmployee(int empId, int month, int year)
        {
            if (month < 1 || month > 12)
                return BadRequest("Tháng không hợp lệ.");

            try
            {
                var data = await _clockinService.GetClockinsByEmployeeAsync(empId, month, year);

                if (data == null || data.Count == 0)
                    return NotFound("Không tìm thấy dữ liệu chấm công cho nhân viên này.");

                return Ok(data); // trả về JSON cho lịch
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }
    }
}