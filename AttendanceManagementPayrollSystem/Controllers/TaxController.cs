using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet("active")]
        public async Task<ActionResult<TaxDTO?>> GetActive()
        {
            var result = await taxService.GetActiveTaxDTOs();
            if (result == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(result);
            }
        }

        [HttpPost("edit")]
        public async Task<ActionResult<TaxDTO>> EditTax([FromBody] TaxEditDTO dto)
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


        // GET api/<TaxController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<TaxController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<TaxController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<TaxController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
