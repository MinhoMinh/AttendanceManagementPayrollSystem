using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface DepartmentService
    {
        Task<IEnumerable<DepartmentDTO>> GetAllAsync();
        Task<DepartmentDTO?> GetByIdAsync(int id);
        Task<IEnumerable<EmployeeBasicDTO>> GetEmployeesAsync(int headId);
        Task<Dictionary<string, List<EmployeeBasicDTO>>> GetEmployeesByDepAsync();
        Task<IEnumerable<DepartmentDTO>> GetAllDepartmentExceptManager();
    }
}
