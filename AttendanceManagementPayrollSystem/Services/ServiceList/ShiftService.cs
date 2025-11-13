using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface ShiftService
    {
        Task<WeeklyShiftDto?> GetWeeklyShiftDto(int empId);

        Task<Dictionary<int, WeeklyShiftDto?>> GetWeeklyShiftDtos(IEnumerable<int> empIds);

        Task<DailyShiftAfterCreateDTO> CreateAsync(DailyShiftCreateDTO dto);
        Task<List<DailyShiftViewDTO>> GetAllForViewAsync();

        Task<DailyShiftViewDTO?> GetByIdAsync(int id);

        Task<List<WeeklyShiftViewDTO>> GetAllWeeklyShiftAsync();
        Task UpdateWeeklyShiftAsync(int id, WeeklyShiftCreateUpdateDTO dto);

    }
}
