using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceManagementPayrollSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalaryPolicyController : ControllerBase
    {
        private readonly SalaryPolicyService salaryPolicyService;

        public SalaryPolicyController(SalaryPolicyService salaryPolicyService)
        {
            this.salaryPolicyService = salaryPolicyService;
        }

        // GET: api/<SalaryPolicyController>
        [HttpGet]
        public async Task<ActionResult<List<SalaryPolicyViewDTO>>> GetAll()
        {
            var policies = await salaryPolicyService.GetAllAsync();
            if (policies == null)
            {
                return NotFound();
            }
            return Ok(policies);
        }
        [HttpGet("getactivesalarypolicy")]
        public async Task<ActionResult<List<SalaryPolicyViewDTO>>> GetActiveSalaryPolicy()
        {
            var activePolicy = await salaryPolicyService.GetActiveSalaryPolicyAsync();
            if (activePolicy == null)
            {
                return NotFound();
            }
            return Ok(activePolicy);
        }

        [HttpGet("getinactivesalarypolicy")]
        public async Task<ActionResult<List<SalaryPolicyViewDTO>>> GetInactiveSalaryPolicy()
        {
            var activePolicy = await salaryPolicyService.GetInactiveSalaryPolicyAsync();
            if (activePolicy == null)
            {
                return NotFound();
            }
            return Ok(activePolicy);
        }


        // GET api/<SalaryPolicyController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<SalaryPolicyController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<SalaryPolicyController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<SalaryPolicyViewDTO>> Update(int id, [FromBody] SalaryPolicyEditDTO dto) { 
            if (id != dto.SalId)
            {
                return BadRequest("ID in URL does not match ID in body");
            }

            try
            {
                var updatedPolicy = await salaryPolicyService.UpdateSalaryPolicyAsync(dto);
                return Ok(updatedPolicy);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE api/<SalaryPolicyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
