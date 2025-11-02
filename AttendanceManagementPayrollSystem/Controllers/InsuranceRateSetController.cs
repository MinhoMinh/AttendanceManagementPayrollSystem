using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceManagementPayrollSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsuranceRateSetController : ControllerBase
    {
        private readonly InsuranceRateService insuranceRateService;

        public InsuranceRateSetController(InsuranceRateService insuranceRateService)
        {
            this.insuranceRateService = insuranceRateService;
        }


        // GET: api/<InsuranceRateSetController>
        [HttpGet]
        public async Task<ActionResult<List<InsuranceRateDTO>>> Get()
        {
            var result = await this.insuranceRateService.GetInsuranceRateSetDTOs();
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        [HttpGet("active")]
        public async Task<ActionResult<List<InsuranceRateDTO>>> GetActive()
        {
            var result = await this.insuranceRateService.GetActiveInsuranceRateSetDTO();
            if (result == null)
            {
                return NotFound();
            }
            return result;
        }
        [HttpGet("active/ids")]
        public async Task<ActionResult<List<int>>> GetActiveIds()
        {
            var ids = await this.insuranceRateService.GetActiveIds();
            if (ids == null)
            {
                return NotFound();
            }
            return Ok(ids);
        }

        [HttpPut("active/{id}")]
        public async Task<IActionResult> UpdateInsuranceRate(int id, [FromBody] InsuranceRateDTO dto)
        {
            if (id != dto.RateSetId)
            {
                return BadRequest("ID mismatch");
            }
            var result = await this.insuranceRateService.UpdateActiveAsync(dto);
            if (result == false)
            {
                return NotFound();
            }
            return NoContent();
        }



        // GET api/<InsuranceRateSetController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<InsuranceRateSetController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }


        // DELETE api/<InsuranceRateSetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
