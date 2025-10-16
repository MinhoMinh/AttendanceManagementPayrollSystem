using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface EmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<IEnumerable<Employee>> GetByDepartment(int depId, params Expression<Func<Employee, object>>[] includes);
        Task<IEnumerable<Employee>> GetByDepartmentWithKPI(int depId);
        Task<Employee?> GetByIdAsync(int id);
        Task<Employee> AddAsync(Employee emp);
        Task<Employee?> UpdateAsync(Employee emp);
        Task<Employee?> FindByUsernameAsync(string username); // ✅ Thêm mới
        Task LoadRoles(Employee employee);
        Task <int> GetIdByClockId(int clockId);

    }
}
