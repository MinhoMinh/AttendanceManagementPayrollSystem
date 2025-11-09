using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface ShiftService
    {
        Task<WeeklyShiftDto?> GetWeeklyShiftDto(int empId);

        Task<Dictionary<int, WeeklyShiftDto?>> GetWeeklyShiftDtos(IEnumerable<int> empIds);

    }
}
