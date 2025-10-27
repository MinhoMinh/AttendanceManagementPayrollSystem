using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface EmployeeDependentService
    {
        Task<IEnumerable<EmployeeDependentDTO>> GetAllAsync();
        Task<IEnumerable<EmployeeDependentDTO>> GetByEmployeeIdAsync(int employeeId);
        Task<EmployeeDependentDTO?> GetByIdAsync(int id);
        Task<EmployeeDependentDTO> CreateAsync(CreateEmployeeDependentDTO dto, int createdBy);
        Task<EmployeeDependentDTO?> UpdateAsync(UpdateEmployeeDependentDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
