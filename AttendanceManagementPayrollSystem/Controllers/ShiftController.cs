using System.Data;
using Microsoft.AspNetCore.Mvc;
using ExcelDataReader;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/shift")]
    public class ShiftController : ControllerBase
    {
        private readonly ShiftService _shiftService;

        public ShiftController(ShiftService shiftService)
        {
            _shiftService = shiftService;
        }

        [HttpGet("self")]
        public async Task<ActionResult<WeeklyShiftDto>> GetShiftBySelf([FromQuery] int empId)
        {
            var shift = await _shiftService.GetWeeklyShiftDto(empId);

            if (shift == null)
                return NotFound();

            return Ok(shift);
        }
        [HttpGet("dailyshiftlist")]
        public async Task<ActionResult<List<DailyShiftViewDTO>>> GetDailyShiftList()
        {
            var dsList = await this._shiftService.GetAllForViewAsync();

            if (dsList == null)
            {
                return NotFound();
            }
            return Ok(dsList);
        }

        [HttpGet("weeklyshiftlist")]
        public async Task<ActionResult<List<WeeklyShiftViewDTO>>> GetWeeklyShiftList()
        {
            var wsList = await this._shiftService.GetAllWeeklyShiftAsync();
            if (wsList == null)
            {
                return NotFound();
            }
            return Ok(wsList);
        }

        [HttpPost("createnewdailyshift")]
        public async Task<ActionResult<DailyShiftCreateDTO>> CreateNewDailyShift([FromBody] DailyShiftCreateDTO dto)
        {
            if (dto == null || dto.Segments == null || !dto.Segments.Any())
            {
                return BadRequest("Segments cannot be empty");
            }

            try
            {
                var result = await this._shiftService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = result.ShiftId }, result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeeklyShift(int id, [FromBody] WeeklyShiftCreateUpdateDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await this._shiftService.UpdateWeeklyShiftAsync(id, dto);
                return Ok(new { message = "Weekly shift updated successfully" });
            }
            catch (Exception ex)
            {

                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DailyShiftAfterCreateDTO>> GetById(int id)
        {
            var shift = await _shiftService.GetByIdAsync(id);
            if (shift == null)
                return NotFound();

            return Ok(shift);
        }
    }
}
