using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{

    [ApiController]
    [Route("api/clockin-adjustments")]
    public class ClockInAdjustmentRequestController : Controller
    {
        private readonly ClockInAdjustmentRequestService clockInAdjustmentRequestService;

        public ClockInAdjustmentRequestController(ClockInAdjustmentRequestService clockInAdjustmentRequestService)
        {
            this.clockInAdjustmentRequestService = clockInAdjustmentRequestService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ClockinAdjustmentRequestCreateDTO dto)
        {
            if (dto == null)
                return BadRequest("Invalid data");

            var success = await this.clockInAdjustmentRequestService.AddClockInAdjustmentRequest(dto);
            if (!success)
                return StatusCode(500, "Failed to create adjustment request");

            return Ok(new { message = "Clock-in adjustment request created successfully" });
        }

        [HttpGet("groupbydep")]
        public async Task<ActionResult<List<IGrouping<int?, ClockInAdjustmentRequest>>>> GetClockInAdjustmentRequestsByDep()
        {
            var result = await this.clockInAdjustmentRequestService.GetClockinAdjustmentRequestGroupByDepID();
            return Ok(result);
        }

        [HttpGet("groupbydep/daterange")]
        public async Task<IActionResult> GetGroupByDepIdAndDateRange([FromQuery] DateTime from, [FromQuery] DateTime to)
        {
            var result = await this.clockInAdjustmentRequestService.GetClockinAdjustmentRequestGroupByDepIdAndDateRange(from, to);
            return Ok(result);
        }

        [HttpPost("respond")]
        public async Task<IActionResult> Respond([FromBody] ClockinAdjustmentRespondDTO dto)
        {
            if (dto == null || dto.RequestId <= 0)
            {
                return BadRequest("Invalid request data.");
            }
            var result = await this.clockInAdjustmentRequestService.RespondAsync(dto);
            if (!result)
            {
                return StatusCode(500, "Failed to update request.");
            }
            return Ok(new { message = "Response updated successfully" });
        }

    }
}
