using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentWeeklyShiftController : ControllerBase
    {
        private readonly DepartmentWeeklyShiftService _service;

        public DepartmentWeeklyShiftController(DepartmentWeeklyShiftService service)
        {
            _service = service;
        }

        // GET: api/departmentweeklyshift
        [HttpGet]
        public async Task<ActionResult<List<DepartmentWeeklyShiftViewDTO>>> GetAll()
        {
            var data = await _service.GetAllForViewAsync();
            return Ok(data);
        }

        // GET: api/departmentweeklyshift/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<DepartmentWeeklyShiftViewDTO>> GetById(int id)
        {
            var data = await _service.GetByIdForViewAsync(id);
            if (data == null) return NotFound();
            return Ok(data);
        }

        // PUT: api/departmentweeklyshift
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] DepartmentWeeklyShiftUpdateDTO dto)
        {
            if (dto == null) return BadRequest();

            var success = await _service.UpdateAsync(dto);
            if (!success) return NotFound(); // không tìm thấy record để update

            return NoContent(); // trả về 204 khi update thành công
        }
    }
}
