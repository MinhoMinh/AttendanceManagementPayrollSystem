using AttendanceManagementPayrollSystem.Models;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface IEmployeeBalanceRepository
    {
        Task<EmployeeBalance?> GetByEmployeeIdAsync(int empId);
    }
}
