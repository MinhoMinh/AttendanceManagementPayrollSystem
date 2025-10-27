using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services;
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

        // GET: api/employeedependent
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeDependentDTO>>> GetAll()
        {
            var dependents = await _service.GetAllAsync();
            return Ok(dependents);
        }

        // GET: api/employeedependent/employee/5
        [HttpGet("employee/{employeeId}")]
        public async Task<ActionResult<IEnumerable<EmployeeDependentDTO>>> GetByEmployeeId(int employeeId)
        {
            var dependents = await _service.GetByEmployeeIdAsync(employeeId);
            return Ok(dependents);
        }

        // GET: api/employeedependent/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeDependentDTO>> GetById(int id)
        {
            var dependent = await _service.GetByIdAsync(id);
            if (dependent == null)
                return NotFound();

            return Ok(dependent);
        }

        // POST: api/employeedependent
        [HttpPost]
        public async Task<ActionResult<EmployeeDependentDTO>> Create([FromBody] CreateEmployeeDependentDTO dto, [FromQuery] int createdBy)
        {
            try
            {
                var created = await _service.CreateAsync(dto, createdBy);
                return CreatedAtAction(nameof(GetById), new { id = created.DependentId }, created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // PUT: api/employeedependent/5
        [HttpPut("{id}")]
        public async Task<ActionResult<EmployeeDependentDTO>> Update(int id, [FromBody] UpdateEmployeeDependentDTO dto)
        {
            if (id != dto.DependentId)
                return BadRequest("ID mismatch");

            try
            {
                var updated = await _service.UpdateAsync(dto);
                if (updated == null)
                    return NotFound();

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // DELETE: api/employeedependent/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
