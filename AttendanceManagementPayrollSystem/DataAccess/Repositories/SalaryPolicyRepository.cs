using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface SalaryPolicyRepository:BaseRepository
    {
        Task<IEnumerable<SalaryPolicyViewDTO>> GetAllAsync();
        Task<SalaryPolicy> UpdateSalaryPolicyAsync(SalaryPolicy updatedSalaryPolicy);
        Task<SalaryPolicyViewDTO?> GetActiveSalaryPolicyAsync();
        Task<List<SalaryPolicyViewDTO>> GetInactiveSalaryPolicyAsync();

    }
}
