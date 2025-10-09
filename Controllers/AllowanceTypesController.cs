using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AllowanceTypesController : ControllerBase
    {
        private readonly AllowanceTypeRepository _repo;

        public AllowanceTypesController(AllowanceTypeRepository repo)
        {
            _repo = repo;
        }

        // GET: api/allowancetypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AllowanceType>>> GetAllowanceTypes()
        {
            var allowanceTypes = await _repo.GetAllAsync();
            return Ok(allowanceTypes);
        }

        // GET: api/allowancetypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AllowanceType>> GetAllowanceType(int id)
        {
            var allowanceType = await _repo.GetByIdAsync(id);

            if (allowanceType == null)
            {
                return NotFound();
            }

            return Ok(allowanceType);
        }

        // GET: api/allowancetypes/search?term=searchTerm
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<AllowanceType>>> SearchAllowanceTypes([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return BadRequest("Search term cannot be empty");
            }

            var allowanceTypes = await _repo.SearchAsync(term);
            return Ok(allowanceTypes);
        }

        // POST: api/allowancetypes
        [HttpPost]
        public async Task<ActionResult<AllowanceType>> CreateAllowanceType(AllowanceType allowanceType)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdAllowanceType = await _repo.AddAsync(allowanceType);
            return CreatedAtAction(nameof(GetAllowanceType), new { id = createdAllowanceType.TypeId }, createdAllowanceType);
        }

        // PUT: api/allowancetypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAllowanceType(int id, AllowanceType allowanceType)
        {
            if (id != allowanceType.TypeId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedAllowanceType = await _repo.UpdateAsync(allowanceType);
            if (updatedAllowanceType == null)
            {
                return NotFound();
            }

            return Ok(updatedAllowanceType);
        }

        // DELETE: api/allowancetypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAllowanceType(int id)
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