using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BonusController : ControllerBase
    {
        private readonly BonusService _service;

        public BonusController(BonusService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BonusDTO>>> GetAllAsync()
        {
            var list = await _service.GetAllAsync();
            return Ok(list);
        }

        [HttpPost("assign")]
        public async Task<IActionResult> AssignAsync([FromBody] AssignBonusRequest request)
        {
            // In a real app, get current user id from auth; here default to 1
            int createdBy = 1;
            await _service.AssignAsync(request, createdBy);
            return Ok(new { success = true });
        }

        [HttpGet("department/{depId:int}")]
        public async Task<ActionResult<DepartmentBonusViewDTO>> GetByDepartmentAsync([FromRoute] int depId)
        {
            try
            {
                var result = await _service.GetByDepartmentAsync(depId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                // degrade gracefully to avoid UI crash
                return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
            }
        }
    }
}

