using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentHolidayCalendarController : ControllerBase
    {
        private readonly DepartmentHolidayCalendarRepository _repo;

        public DepartmentHolidayCalendarController(DepartmentHolidayCalendarRepository repo)
        {
            _repo = repo;
        }

        // ✅ GET: api/departmentholidaycalendar
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.GetAllAsync();
            return Ok(items);
        }

        // ✅ GET: api/departmentholidaycalendar/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.GetDtoByIdAsync(id);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // ✅ POST: api/departmentholidaycalendar
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] DepartmentHolidayCalendarCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Kiểm tra tồn tại (không cho trùng phòng ban & holiday)
            bool exists = await _repo.ExistsAsync(dto.DepId, dto.HolidayId);
            if (exists)
                return Conflict(new { message = "Department đã được gán kỳ nghỉ này rồi." });

            var entity = new DepartmentHolidayCalender
            {
                DepId = dto.DepId,
                HolidayId = dto.HolidayId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            var saved = await _repo.AddAsync(entity);

            var result = new DepartmentHolidayCalendarDTO
            {
                DepHolidayCalendarId = saved.DepHolidayCalendarId,
                DepId = saved.DepId ?? 0,
                HolidayId = saved.HolidayId ?? 0,
                StartDate = saved.StartDate,
                EndDate = saved.EndDate
            };

            return CreatedAtAction(nameof(GetById), new { id = saved.DepHolidayCalendarId }, result);
        }

        // ✅ PUT: api/departmentholidaycalendar/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] DepartmentHolidayCalendarCreateDTO dto)
        {
            var existing = await _repo.GetDtoByIdAsync(id);
            if (existing == null)
                return NotFound();

            var entity = new DepartmentHolidayCalender
            {
                DepHolidayCalendarId = id,
                DepId = dto.DepId,
                HolidayId = dto.HolidayId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };

            await _repo.UpdateAsync(entity);
            return NoContent();
        }
    }
}
