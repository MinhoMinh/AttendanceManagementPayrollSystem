using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Blazorise.Extensions;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

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

        [HttpGet("group")]
        public async Task<ActionResult<List<InsuranceRateGroupDTO>>> GetInsuranceRateGroupsAsync()
        {
            var result = await this.insuranceRateService.GetInsuranceRateGroupsAsync();
            if (result.IsNullOrEmpty()) return NotFound();
            return result;
        }

        [HttpDelete("upcoming/{id}")]
        public async Task<IActionResult> RemoveUpcomingInsuranceAsync(int id)
        {
            try
            {
                await this.insuranceRateService.RemoveUpcomingInsuranceByIdAsync(id);
                return Ok(new { message = "Upcoming insurance rate removed successfully." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Insurance rate not found." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("upcoming/{id}")]
        public async Task<IActionResult> UpdateUpcomingInsuranceRate(int id, [FromBody] InsuranceRateDTO dto)
        {
            if (id != dto.RateSetId)
            {
                return BadRequest("ID mismatch");
            }
            var result = await this.insuranceRateService.UpdateUpcomingAsync(dto);
            if (result == false)
            {
                return NotFound();
            }
            return NoContent();
        }
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatusAsync(int id, [FromBody] bool isActive)
        {
            try
            {
                await insuranceRateService.UpdateStatusAsync(id, isActive);
                return Ok(new { message = "Cập nhật trạng thái thành công." });
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { message = "Không tìm thấy bản ghi." });
            }
        }
        [HttpPost]
        public async Task<IActionResult> AddInsuranceRateAsync([FromBody] InsuranceRateDTO insuranceRateDTO)
        {
            if (insuranceRateDTO == null)
            {
                return BadRequest("Invalid data");
            }
            var result = await this.insuranceRateService.AddInsuranceRate(insuranceRateDTO);
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



        // DELETE api/<InsuranceRateSetController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
