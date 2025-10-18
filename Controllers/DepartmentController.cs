using AttendanceManagementPayrollSystem.DTO;
using Microsoft.AspNetCore.Mvc;
using AttendanceManagementPayrollSystem.Services;

[Route("api/[controller]")]
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
}
