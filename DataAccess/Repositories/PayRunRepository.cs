using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface PayRunRepository : BaseRepository
    {
        Task<SalaryPolicy?> GetActivePolicyAsync(int periodMonth, int periodYear);

        Task<PayRun> AddAsync(PayRun run);

        Task<PayRun?> FindAsync(int payrollId);

        Task<IEnumerable<PayRun>> GetAllAsync();

        Task<PayRunDto?> GetDtoAsync(int payrollId);

        Task Update(PayRun run);

        Task<List<Employee>> GetEmployeesWithComponents(int month, int year);

        Task<bool> ContainsValidPayRunInPeriod(int month, int year);

        Task SaveRegularPayRun(PayRun run);
    }
}
