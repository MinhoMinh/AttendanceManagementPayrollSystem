using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ShiftRepository : BaseRepository
    {
        Task<WeeklyShift?> GetWeeklyShift(int empId);
        Task<Dictionary<int, WeeklyShift?>> GetWeeklyShifts(IEnumerable<int> empIds);
    }
}
