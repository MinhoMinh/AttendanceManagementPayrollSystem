using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface EmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee emp);
        Task<Employee?> UpdateAsync(Employee emp);
        Task<Employee?> FindByUsernameAsync(string username); // ✅ Thêm mới
    }
}
