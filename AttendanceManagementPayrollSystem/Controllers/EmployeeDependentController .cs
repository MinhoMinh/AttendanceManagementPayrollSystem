using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDependentController : ControllerBase
    {
        private readonly EmployeeDependentService _service;

        public EmployeeDependentController(EmployeeDependentService service)
        {
            _service = service;
        }

        [HttpGet("grouped")]
        public async Task<IActionResult> GetDependentsGroupedByEmployee()
        {
            var result = await _service.GetDependentsGroupedByEmployeeAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddDependent([FromBody] EmployeeDependentCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var created = await _service.AddDependentAsync(dto);
            return CreatedAtAction(nameof(GetDependentsGroupedByEmployee), new { id = created.DependentId }, created);
        }
    }
}
