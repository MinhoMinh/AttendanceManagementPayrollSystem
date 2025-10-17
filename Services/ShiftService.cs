using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface ShiftService
    {
        Task<WeeklyShiftDto?> GetWeeklyShiftDto(int empId);

        Task<Dictionary<int, WeeklyShiftDto?>> GetWeeklyShiftDtos(IEnumerable<int> empIds);

    }
}
