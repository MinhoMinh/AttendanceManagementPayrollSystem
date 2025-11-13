using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ShiftRepository : BaseRepository
    {
        Task<WeeklyShift?> GetWeeklyShift(int empId);
        Task<Dictionary<int, WeeklyShift?>> GetWeeklyShifts(IEnumerable<int> empIds);

        Task<DailyShift> AddShiftAsync(DailyShift dailyShift);

        Task<List<DailyShift>> GetAllDailyShiftAsync();

        Task<DailyShift?> GetDailyShiftByIdAsync(int id);

        Task<List<WeeklyShift>> GetAllWeeklyShiftAsync();

        Task<WeeklyShift?> GetWeeklyShiftById(int id);
        Task UpdateWeeklyShiftAsync(WeeklyShift weeklyShift);
    }
}
