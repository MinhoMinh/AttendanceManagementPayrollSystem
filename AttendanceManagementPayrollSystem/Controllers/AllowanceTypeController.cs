using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllowanceTypeController : ControllerBase
    {
        private readonly AllowanceTypeService _allowanceService;

        public AllowanceTypeController(AllowanceTypeService allowanceService)
        {
            _allowanceService = allowanceService;
        }

        /// <summary>
        /// Lấy toàn bộ loại phụ cấp
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var data = await _allowanceService.GetAllAsync();
                if (data == null || !data.Any())
                    return NotFound("Không có dữ liệu phụ cấp.");

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy chi tiết một loại phụ cấp theo ID
        /// </summary>
        [HttpGet("{typeId}")]
        public async Task<IActionResult> GetById(int typeId)
        {
            if (typeId <= 0)
                return BadRequest("ID không hợp lệ.");

            try
            {
                var data = await _allowanceService.GetByIdAsync(typeId);
                if (data == null)
                    return NotFound($"Không tìm thấy phụ cấp với ID {typeId}.");

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
            }
        }

        ///// <summary>
        ///// Tạo mới một loại phụ cấp
        ///// </summary>
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] AllowanceTypeDTO dto)
        //{
        //    if (dto == null || string.IsNullOrWhiteSpace(dto.TypeName))
        //        return BadRequest("Dữ liệu phụ cấp không hợp lệ.");

        //    try
        //    {
        //        var created = await _allowanceService.CreateAsync(dto);
        //        return CreatedAtAction(nameof(GetById), new { typeId = created.TypeId }, created);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, $"Lỗi hệ thống: {ex.Message}");
        //    }
        //}

        // POST api/allowancetype
        [HttpPost]
        public async Task<IActionResult> AddAllowanceType([FromBody] AllowanceType model)
        {
            if (model == null)
                return BadRequest();

            var result = await _allowanceService.AddAllowanceTypeAsync(model); // service xử lý lưu DB
            return Ok(result);
        }
    }
}
