using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface EmployeeDependentService
    {
        Task<List<EmployeeWithDependentsDTO>> GetDependentsGroupedByEmployeeAsync();
        Task<EmployeeDependentDTO> AddDependentAsync(EmployeeDependentCreateDTO dto);
    }
}
