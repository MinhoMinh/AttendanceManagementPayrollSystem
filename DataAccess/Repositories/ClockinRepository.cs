using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ClockinRepository : BaseRepository
    {

        Task<IEnumerable<Clockin>> GetByDateRangeAsync(int periodMonth, int periodYear);
    }
}
