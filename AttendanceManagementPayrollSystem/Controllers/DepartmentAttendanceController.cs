using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Microsoft.AspNetCore.Mvc;

namespace DepartmentManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagerAttendanceApiController : ControllerBase
    {
        private readonly IAttendanceService _attendanceService;

        public ManagerAttendanceApiController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
        }

        // GET: api/ManagerAttendanceApi/summary
        [HttpGet("summary")]
        public ActionResult<List<DepartmentAttendanceDTO>> GetDepartmentAttendanceSummary([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var summary = _attendanceService.GetDepartmentAttendanceSummary(startDate, endDate);

            if (!summary.Any())
                return NotFound(new { message = "No attendance data found." });

            return summary;
        }
        [HttpGet("allclockins")]
        public ActionResult<List<ClockinDTO>> GetAllClockins([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var clockins = _attendanceService.GetAllClockins(startDate, endDate);
            if (!clockins.Any())
                return NotFound(new { message = "No clockin data found." });
            return clockins;
        }

    }
}

