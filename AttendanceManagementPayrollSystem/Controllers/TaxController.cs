using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Reflection.Metadata.Ecma335;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceManagementPayrollSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly TaxService taxService;

        public TaxController(TaxService taxService)
        {
            this.taxService = taxService;
        }


        // GET: api/<TaxController>
        [HttpGet]
        public async Task<ActionResult<List<TaxDTO>>> Get()
        {
            var result = await taxService.GetAllTaxDTOs();
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpGet("group")]
        public async Task<ActionResult<TaxGroupDTO>> GetGroup()
        {
            var result = await taxService.GetTaxGroupAsync();
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("active")]
        public async Task<ActionResult<TaxDTO?>> GetActive()
        {
            var result = await taxService.GetActiveTaxDTO();
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TaxDTO>> AddTax([FromBody] TaxEditDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await this.taxService.AddTaxAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Lỗi khi tạo bản ghi thuế mới: {ex.Message}");
            }
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool newStatus)
        {
            var result = await this.taxService.UpdateStatusAsync(id, newStatus);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("upcoming/{id}")]
        public async Task<IActionResult> DeleteUpcoming(int id)
        {
            var result = await this.taxService.DeleteUpcomingAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] TaxEditDTO dto)
        {
            if (dto == null || id != dto.TaxId)
            {
                return BadRequest("Invalid request data.");
            }
            var result = await this.taxService.UpdateTaxAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

    }
}
