using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface EmployeeDependentRepository
    {
        Task<IEnumerable<EmployeeDependent>> GetAllAsync();
        Task<IEnumerable<EmployeeDependent>> GetByEmployeeIdAsync(int employeeId);
        Task<EmployeeDependent?> GetByIdAsync(int id);
        Task<EmployeeDependent> AddAsync(EmployeeDependent dependent, int createdBy);
        Task<EmployeeDependent?> UpdateAsync(EmployeeDependent dependent);
        Task<bool> DeleteAsync(int id);
    }
}
