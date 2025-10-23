using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/payrunitem")]
    public class PayRunItemController : ControllerBase
    {
        private readonly PayRunItemService _service;

        public PayRunItemController(PayRunItemService payRunItemService)
        {
            this._service = payRunItemService;
        }
        [HttpGet("{empId}")]
        public async Task<IActionResult> TestGetPayRunItems(int empId)
        {
            var result = await _service.GetPayRunItemsByEmpIdAsync(empId);
            return Ok(result);
        }

    }
}
