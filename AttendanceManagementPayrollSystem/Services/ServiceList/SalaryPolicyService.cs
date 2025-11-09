using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface SalaryPolicyService
    {
        Task<IEnumerable<SalaryPolicyViewDTO>> GetAllAsync();
        Task<SalaryPolicyViewDTO> UpdateSalaryPolicyAsync(SalaryPolicyEditDTO dto);
        Task<SalaryPolicyViewDTO?> GetActiveSalaryPolicyAsync();
        Task<List<SalaryPolicyViewDTO>> GetInactiveSalaryPolicyAsync();

    }
}
