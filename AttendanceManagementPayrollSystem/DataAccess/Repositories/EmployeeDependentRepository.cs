using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface EmployeeDependentRepository : BaseRepository
    {
        Task<List<EmployeeWithDependentsDTO>> GetDependentsGroupedByEmployeeAsync();
        Task<EmployeeDependentDTO> AddDependentAsync(EmployeeDependentCreateDTO dto);
    }
}
