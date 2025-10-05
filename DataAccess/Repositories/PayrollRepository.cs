using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface PayrollRepository : BaseRepository
    {
        //Task<PayrollRun> CreateRunAsync(string name, int periodMonth, int periodYear, int createdBy);

        Task<PayrollRun> AddAsync(PayrollRun run);

    }
}
