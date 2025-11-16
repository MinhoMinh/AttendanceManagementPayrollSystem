using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly EmployeeRepository _repo;

        public EmployeesController(EmployeeRepository repo)
        {
            _repo = repo;
        }

        // GET: api/employees
        [HttpGet]
        public async Task<IActionResult> GetEmployees()
        {
            var users = await _repo.GetAllAsync();
            return Ok(users);
        }

        // GET: api/employees/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployee(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        [HttpGet("ewd/{id}")]
        public async Task<IActionResult> GetEmployeeWithDep(int id)
        {
            var user = await _repo.GetByIdForDepAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }


        // POST: api/users
        //[HttpPost]
        //public async Task<IActionResult> CreateEmployee(Employee emp)
        //{
        //    await _repo.UpdateAsync(emp);
        //}
    }
}
