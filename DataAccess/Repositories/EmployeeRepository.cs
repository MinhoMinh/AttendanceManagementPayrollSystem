using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface EmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee emp);
        Task<Employee?> UpdateAsync(Employee emp);
    }
}
