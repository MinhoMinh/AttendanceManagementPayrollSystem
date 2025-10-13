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
            Console.WriteLine($"Hit SaveSelfScore for empId={empId}");
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

    }
}
