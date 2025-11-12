using AttendanceManagementPayrollSystem.DTO;
using Microsoft.AspNetCore.Mvc;
using AttendanceManagementPayrollSystem.Services.ServiceList;

[Route("api/department")]
[ApiController]
public class DepartmentController : ControllerBase
{
    private readonly DepartmentService _service;

    public DepartmentController(DepartmentService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<ActionResult<List<DepartmentDTO>>> GetAll()
    {
        var list = await _service.GetAllAsync();
        return Ok(list);
    }

    [HttpGet("employees")]
    public async Task<ActionResult<List<EmployeeBasicDTO>>> GetEmployeesByHeadAsync([FromQuery] int headId)
    {
        // Optional: validate month/year

        var employees = await _service.GetEmployeesAsync(headId);

        if (employees == null)
            return NotFound();

        return Ok(employees);
    }

    [HttpGet("groups")]
    public async Task<ActionResult<List<DepartmentEmployeeGroupDTO>>> GetDepartmentsWithEmployees()
    {
        var groups = await (_service as DepartmentServiceImpl)!.GetAllWithEmployeesAsync();
        return Ok(groups);
    }
}
