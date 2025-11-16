using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeAllowanceController : ControllerBase
    {
        private readonly EmployeeAllowanceService _employeeAllowanceService;

        public EmployeeAllowanceController(EmployeeAllowanceService employeeAllowanceService)
        {
            _employeeAllowanceService = employeeAllowanceService;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách phụ cấp của nhân viên
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _employeeAllowanceService.GetAllAsync();
                if (data == null || !data.Any())
                    return NotFound("Không có dữ liệu phụ cấp nhân viên.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách nhân viên đang nhận một loại phụ cấp cụ thể
        /// </summary>
        [HttpGet("by-type/{typeId}")]
        public async Task<IActionResult> GetByType(int typeId)
        {
            if (typeId <= 0)
                return BadRequest("TypeId không hợp lệ.");

            try
            {
                var data = await _employeeAllowanceService.GetEmployeesByAllowanceTypeAsync(typeId);
                if (data == null || !data.Any())
                    return NotFound($"Không có nhân viên nào có phụ cấp loại ID {typeId}.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách phụ cấp của một nhân viên
        /// </summary>
        [HttpGet("by-employee/{empId}")]
        public async Task<IActionResult> GetByEmployee(int empId)
        {
            if (empId <= 0)
                return BadRequest("EmpId không hợp lệ.");

            try
            {
                var data = await _employeeAllowanceService.GetByEmployeeIdAsync(empId);
                if (data == null || !data.Any())
                    return NotFound($"Không có phụ cấp cho nhân viên ID {empId}.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy chi tiết một bản ghi phụ cấp
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest("ID không hợp lệ.");

            try
            {
                var data = await _employeeAllowanceService.GetByIdAsync(id);
                if (data == null)
                    return NotFound($"Không tìm thấy phụ cấp nhân viên ID {id}.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        /// <summary>
        /// Thêm mới phụ cấp cho nhân viên
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] EmployeeAllowanceCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Dữ liệu không hợp lệ.");

            try
            {
                await _employeeAllowanceService.AddAsync(dto);
                return Ok("Thêm phụ cấp nhân viên thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật phụ cấp nhân viên
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] EmployeeAllowanceCreateDTO dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest("Dữ liệu không hợp lệ hoặc ID không khớp.");

            try
            {
                await _employeeAllowanceService.UpdateAsync(dto);
                return Ok("Cập nhật phụ cấp nhân viên thành công.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa phụ cấp nhân viên theo ID
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest("ID không hợp lệ.");

            try
            {
                await _employeeAllowanceService.DeleteAsync(id);
                return Ok("Đã xóa phụ cấp nhân viên.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách phụ cấp đang hoạt động tại ngày hiện tại
        /// </summary>
        [HttpGet("active")]
        public async Task<IActionResult> GetActive([FromQuery] DateOnly? date = null)
        {
            try
            {
                var queryDate = date ?? DateOnly.FromDateTime(DateTime.Now);
                var data = await _employeeAllowanceService.GetActiveAllowancesAsync(queryDate);

                if (data == null || !data.Any())
                    return NotFound($"Không có phụ cấp đang hoạt động tại ngày {queryDate}.");

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }
    }
}