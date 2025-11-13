using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface DepartmentWeeklyShiftRepository : BaseRepository
    {
        Task<List<DepartmentWeeklyShiftViewDTO>> GetAllForViewAsync();

        Task<DepartmentWeeklyShiftViewDTO> GetByIdForViewAsync(int deptShiftId);

        Task<bool> UpdateAsync(DepartmentWeeklyShiftUpdateDTO dto);
    }
}
