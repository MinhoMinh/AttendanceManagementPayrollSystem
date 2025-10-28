using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeBalanceController : ControllerBase
    {
        private readonly IEmployeeBalanceService _service;

        public EmployeeBalanceController(IEmployeeBalanceService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeBalanceDto>> GetBalanceById(int id) { 
            var balance =  await _service.GetBalanceAsync(id);
            if (balance == null)
                return NotFound("Employee balance not found");
            return Ok(balance);
        
        }

    }
}

