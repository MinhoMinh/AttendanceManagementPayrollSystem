using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface EmployeeAllowanceRepository : BaseRepository
    {
        Task<IEnumerable<EmployeeAllowance>> GetAllAsync();
        Task<EmployeeAllowance?> GetByIdAsync(int id);
        Task<IEnumerable<EmployeeAllowance>> GetByEmployeeIdAsync(int empId);
        Task<IEnumerable<EmployeeAllowance>> GetByAllowanceTypeAsync(int typeId);
        Task<IEnumerable<EmployeeAllowance>> SearchAsync(string searchTerm, string status = null, DateOnly? startDate = null, DateOnly? endDate = null);
        Task<EmployeeAllowance> AddAsync(EmployeeAllowance employeeAllowance);
        Task<EmployeeAllowance?> UpdateAsync(EmployeeAllowance employeeAllowance);
        Task<bool> DeleteAsync(int id);
    }
}