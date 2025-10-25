using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KpiController : ControllerBase
    {
        private readonly KPIService _service;

        public KpiController(KPIService service)
        {
            _service = service;
        }

        [HttpGet("self")]
        public async Task<ActionResult<KpiDto>> GetKpiBySelf([FromQuery] int empId, [FromQuery] int month, [FromQuery] int year)
        {
            // Optional: validate month/year
            if (month < 1 || month > 12) return BadRequest("Invalid month.");
            if (year < 2000 || year > DateTime.Now.Year) return BadRequest("Invalid year.");

            var employeeKpi = await _service.GetKpiBySelfAsync(empId, month, year);

            if (employeeKpi == null)
                return NotFound();

            return Ok(employeeKpi);
        }

        [HttpPost("self/{empId}/score")]
        public async Task<IActionResult> SaveSelfScore(int empId, [FromBody] KpiDto kpiDto)
        {
            if (kpiDto == null || kpiDto.Components == null || kpiDto.Components.Count == 0)
                return BadRequest("Invalid KPI data.");

            try
            {
                await _service.SaveKpiAsync(empId, kpiDto);
                return Ok(new { message = "Self score saved successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error saving self score: {ex.Message}");
            }
        }

        [HttpGet("manager")]
        public async Task<ActionResult<List<EmployeeBasicDTO>>> GetEmployeesWithKpiAsync([FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12) return BadRequest("Invalid month.");
            if (year < 2000 || year > DateTime.Now.Year) return BadRequest("Invalid year.");


            var employees = await _service.GetEmployeesWithKpiByManagerAsync(month, year);

            if (employees == null)
                return NotFound();

            return Ok(employees);
        }

        [HttpGet("manager/kpi")]
        public async Task<ActionResult<KpiDto>> GetKpiByHead([FromQuery] int empId, [FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12) return BadRequest("Invalid month.");
            if (year < 2000 || year > DateTime.Now.Year) return BadRequest("Invalid year.");

            var employeeKpi = await _service.GetKpiByManagerAsync(empId, month, year);

            if (employeeKpi == null)
                return NotFound();

            return Ok(employeeKpi);
        }

        [HttpGet("head")]
        public async Task<ActionResult<List<EmployeeBasicDTO>>> GetEmployeesWithKpiByHeadAsync([FromQuery] int headId, [FromQuery] int month, [FromQuery] int year)
        {
            // Optional: validate month/year
            if (month < 1 || month > 12) return BadRequest("Invalid month.");
            if (year < 2000 || year > DateTime.Now.Year) return BadRequest("Invalid year.");

            var employees = await _service.GetEmployeesWithKpiByHeadAsync(headId, month, year);

            if (employees == null)
                return NotFound();

            return Ok(employees);
        }

        [HttpGet("head/kpi")]
        public async Task<ActionResult<KpiDto?>> GetKpiByManager([FromQuery] int empId, [FromQuery] int month, [FromQuery] int year)
        {
            if (month < 1 || month > 12) return BadRequest("Invalid month.");
            if (year < 2000 || year > DateTime.Now.Year) return BadRequest("Invalid year.");

            var employeeKpi = await _service.GetKpiByHeadAsync(empId, month, year);

            if (employeeKpi == null)
                return NotFound();

            return Ok(employeeKpi);
        }

        [HttpPost("head/{empId}/edit")]
        public async Task<IActionResult> EditKpi(int empId, [FromBody] KpiDto kpiDto)
        {
            if (kpiDto == null || kpiDto.Components == null || kpiDto.Components.Count == 0)
                return BadRequest("Invalid KPI data.");

            try
            {
                await _service.EditKpiAsync(empId, kpiDto);
                return Ok(new { message = "Edit kpi saved successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error editing kpi: {ex.Message}");
            }
        }

        [HttpPost("head/{empId}/assign")]
        public async Task<IActionResult> SaveAssignedScore(int empId, [FromBody] KpiDto kpiDto)
        {
            if (kpiDto == null || kpiDto.Components == null || kpiDto.Components.Count == 0)
                return BadRequest("Invalid KPI data.");

            try
            {
                await _service.AssignKpiAsync(empId, kpiDto);
                return Ok(new { message = "Edit kpi saved successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error editing kpi: {ex.Message}");
            }
        }
    }
}
