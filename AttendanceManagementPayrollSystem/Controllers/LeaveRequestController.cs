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
            {
                // Lấy tất cả lỗi ra
                var errors = ModelState
                    .Where(ms => ms.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                // Trả về BadRequest với chi tiết lỗi
                return BadRequest(new
                {
                    Message = "Validation failed",
                    Errors = errors
                });
            }

            try
            {
                var created = await _service.AddAsync(dto);
                return Ok(created);
            }
            catch (Exception ex)
            {
                // Bắt lỗi runtime nếu có
                return StatusCode(500, new
                {
                    Message = "Internal server error",
                    Details = ex.Message
                });
            }
        }

        [HttpPut("approve")]
        public async Task<IActionResult> Approve([FromBody] LeaveRequestApprovalDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (dto.ReqId <= 0)
                return BadRequest("Invalid request ID.");

            if (string.IsNullOrWhiteSpace(dto.Status))
                return BadRequest("Status is required.");

            try
            {
                var updated = await _service.UpdateApprovalAsync(dto);

                if (updated == null)
                    return NotFound("Leave request not found.");

                return Ok(new
                {
                    message = "Leave request updated successfully.",
                    data = updated
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "An error occurred while updating leave request.",
                    details = ex.Message
                });
            }
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

        [HttpGet("history/all")]
        public async Task<IActionResult> GetAllLeaveRequestByDate(
            [FromQuery] DateOnly? startDate,
            [FromQuery] DateOnly? endDate)
        {
            var data = await _service.GetAllLeaveRequestByDate(startDate, endDate);
            return Ok(data);
        }

        [HttpGet("groupbydep/daterange")]
        public async Task<IActionResult> GetLeaveRequestsGroupByDep([FromQuery] int depId, [FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            if (from > to)
                return BadRequest("Ngày bắt đầu không được lớn hơn ngày kết thúc.");

            try
            {
                var groups = await _service.GetLeaveRequestGroupByDepIdAndDateRange(depId, from, to);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = "Đã xảy ra lỗi khi tải dữ liệu leave request.",
                    details = ex.Message
                });
            }
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


