using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Microsoft.AspNetCore.Mvc;



namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/clockin-com")]
    public class ClockinComponentController : Controller
    {
        private ClockinComponentService componentService;

        public ClockinComponentController(ClockinComponentService componentService)
        {
            this.componentService = componentService;
        }

        [HttpPost("respond")]
        public async Task<IActionResult> UpdateByRespond([FromBody] ClockinComponentRespondDTO dto)
        {
            if (dto == null || dto.Id <= 0) return BadRequest("Invalid request data.");
            var result = await this.componentService.UpdateByRespond(dto);
            if(!result) return StatusCode(500, "Failed to update request.");
            return Ok(new { message = "Response updated successfully" });
        }
    }
}
