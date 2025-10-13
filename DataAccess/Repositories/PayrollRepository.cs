using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface PayrollRepository : BaseRepository
    {
        Task<SalaryPolicy?> GetActivePolicyAsync(int periodMonth, int periodYear);

        Task<PayrollRun> AddAsync(PayrollRun run);

        Task<PayrollRun?> FindAsync(int payrollId);

        Task<IEnumerable<PayrollRun>> GetAllAsync();

        Task Update(PayrollRun run);
    }
}
