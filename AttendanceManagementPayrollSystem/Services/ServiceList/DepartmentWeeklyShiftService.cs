using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface DepartmentWeeklyShiftService
    {
        Task<List<DepartmentWeeklyShiftViewDTO>> GetAllForViewAsync();

        Task<DepartmentWeeklyShiftViewDTO> GetByIdForViewAsync(int deptShiftId);

        Task<bool> UpdateAsync(DepartmentWeeklyShiftUpdateDTO dto);
    }
}
