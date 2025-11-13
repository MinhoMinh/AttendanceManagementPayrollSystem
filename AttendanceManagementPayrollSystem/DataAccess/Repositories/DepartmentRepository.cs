using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface DepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<IEnumerable<Employee>> GetEmployees(int headId);
        Task<Dictionary<string, List<EmployeeBasicDTO>>> GetEmployeesByDepAsync();
    }
}
