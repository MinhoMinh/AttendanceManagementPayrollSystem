using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeAllowancesController : ControllerBase
    {
        private readonly EmployeeAllowanceRepository _repo;

        public EmployeeAllowancesController(EmployeeAllowanceRepository repo)
        {
            _repo = repo;
        }

        // GET: api/employeeallowances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeAllowance>>> GetEmployeeAllowances()
        {
            var employeeAllowances = await _repo.GetAllAsync();
            return Ok(employeeAllowances);
        }

        // GET: api/employeeallowances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeAllowance>> GetEmployeeAllowance(int id)
        {
            var employeeAllowance = await _repo.GetByIdAsync(id);

            if (employeeAllowance == null)
            {
                return NotFound();
            }

            return Ok(employeeAllowance);
        }

        // GET: api/employeeallowances/employee/5
        [HttpGet("employee/{empId}")]
        public async Task<ActionResult<IEnumerable<EmployeeAllowance>>> GetEmployeeAllowancesByEmployee(int empId)
        {
            var employeeAllowances = await _repo.GetByEmployeeIdAsync(empId);
            return Ok(employeeAllowances);
        }

        // GET: api/employeeallowances/type/5
        [HttpGet("type/{typeId}")]
        public async Task<ActionResult<IEnumerable<EmployeeAllowance>>> GetEmployeeAllowancesByType(int typeId)
        {
            var employeeAllowances = await _repo.GetByAllowanceTypeAsync(typeId);
            return Ok(employeeAllowances);
        }

        // GET: api/employeeallowances/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<EmployeeAllowance>>> SearchEmployeeAllowances(
            [FromQuery] string searchTerm,
            [FromQuery] string status = null,
            [FromQuery] DateOnly? startDate = null,
            [FromQuery] DateOnly? endDate = null)
        {
            if (string.IsNullOrWhiteSpace(searchTerm) && string.IsNullOrWhiteSpace(status) && !startDate.HasValue && !endDate.HasValue)
            {
                return BadRequest("At least one search criteria must be provided");
            }

            var employeeAllowances = await _repo.SearchAsync(searchTerm, status, startDate, endDate);
            return Ok(employeeAllowances);
        }

        // POST: api/employeeallowances
        [HttpPost]
        public async Task<ActionResult<EmployeeAllowance>> CreateEmployeeAllowance(EmployeeAllowance employeeAllowance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdEmployeeAllowance = await _repo.AddAsync(employeeAllowance);
            return CreatedAtAction(
                nameof(GetEmployeeAllowance),
                new { id = createdEmployeeAllowance.Id },
                createdEmployeeAllowance);
        }

        // PUT: api/employeeallowances/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployeeAllowance(int id, EmployeeAllowance employeeAllowance)
        {
            if (id != employeeAllowance.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedEmployeeAllowance = await _repo.UpdateAsync(employeeAllowance);
            if (updatedEmployeeAllowance == null)
            {
                return NotFound();
            }

            return Ok(updatedEmployeeAllowance);
        }

        // DELETE: api/employeeallowances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployeeAllowance(int id)
        {
            var result = await _repo.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}