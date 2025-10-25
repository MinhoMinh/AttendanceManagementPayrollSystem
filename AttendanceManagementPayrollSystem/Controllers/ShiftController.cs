using System.Data;
using Microsoft.AspNetCore.Mvc;
using ExcelDataReader;
using AttendanceManagementPayrollSystem.Services;
using AttendanceManagementPayrollSystem.DTO;

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
    }
}
