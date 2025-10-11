using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KPIController : ControllerBase
    {
        private readonly KPIService _service;

        public KPIController(KPIService service)
        {
            _service = service;
        }

        [HttpGet("{empId}")]
        public async Task<ActionResult<EmployeeWithKpiDTO>> GetEmployeeKpi( int empId, [FromQuery] int month, [FromQuery] int year)
        {
            // Optional: validate month/year
            if (month < 1 || month > 12) return BadRequest("Invalid month.");
            if (year < 2000 || year > DateTime.Now.Year) return BadRequest("Invalid year.");

            var employeeKpi = await _service.GetKpiAsync(empId, month, year);

            if (employeeKpi == null)
                return NotFound();

            return Ok(employeeKpi);
        }

        [HttpPost("{empId}/save")]
        public async Task<IActionResult> SaveEmployeeKpi( int empId, [FromQuery] string phase, [FromBody] EmployeeWithKpiDTO updatedKpi)
        {
            if (updatedKpi == null || updatedKpi.Kpi == null)
                return BadRequest("No KPI data provided.");

            try
            {
                await _service.SaveEmployeeKpiAsync(empId, phase, updatedKpi);
                return Ok(updatedKpi); // return saved data
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }



    }
}
