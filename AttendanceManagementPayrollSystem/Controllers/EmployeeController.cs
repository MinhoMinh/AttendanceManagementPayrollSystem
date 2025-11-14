using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.ServiceList;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AttendanceManagementPayrollSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }


        [HttpGet("basic")]
        public async Task<IActionResult> GetAllEmployeeBasic()
        {
            var employees = await this.employeeService.GetAllEmployeeBasic();
            return Ok(employees);
        }

        [HttpGet("getallforview")]
        public async Task<IActionResult> GetAllForView()
        {
            var employees = await this.employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [HttpGet("getgroupbydepforview")]
        public async Task<IActionResult> GetGroupByDepForView()
        {
            var employees = await this.employeeService.GetEmployeesGroupedByDepartmentAsync();
            return Ok(employees);
        }

        // GET api/<EmployeeController>/5
        [HttpGet("view/{id}")]
        public async Task<IActionResult> GetEmployeeForViewById(int id)
        {
            var employees = await this.employeeService.GetEmployeeViewByIdAsync(id);
            return Ok(employees);
        }

        // POST api/<EmployeeController>
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] EmployeeCreateDTO dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            try
            {
                await employeeService.CreateAsync(dto);
                return Ok(new { message = "Tài khoản mới được tạo thành công" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] EmployeeStatusUpdateDTO dto)
        {
            var success = await this.employeeService.UpdateStatusAsync(dto);
            return success ? Ok() : NotFound();
        }


        // DELETE api/<EmployeeController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
