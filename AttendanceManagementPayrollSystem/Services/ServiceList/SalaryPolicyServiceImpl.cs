using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public class SalaryPolicyServiceImpl : SalaryPolicyService
    {
        private readonly SalaryPolicyRepository salaryPolicyRepository;

        public SalaryPolicyServiceImpl(SalaryPolicyRepository salaryPolicyRepository)
        {
            this.salaryPolicyRepository = salaryPolicyRepository;
        }

        public Task<SalaryPolicyViewDTO?> GetActiveSalaryPolicyAsync()
        {
            return salaryPolicyRepository.GetActiveSalaryPolicyDTOAsync();
        }

        public async Task<IEnumerable<SalaryPolicyViewDTO>> GetAllAsync()
        {
            return await salaryPolicyRepository.GetAllAsync();
        }

        public async Task<List<SalaryPolicyViewDTO>> GetInactiveSalaryPolicyAsync()
        {
            return await salaryPolicyRepository.GetInactiveSalaryPolicyAsync();
        }

        public async Task<SalaryPolicyViewDTO> UpdateSalaryPolicyAsync(SalaryPolicyEditDTO dto)
        {
            var entiy = new SalaryPolicy
            {
                SalId = dto.SalId,
                EffectiveFrom = dto.EffectiveFrom,
                IsActive = dto.IsActive,
                OverclockMultiplier = dto.OverclockMultiplier,
                SalaryPolicyName = dto.SalaryPolicyName,
                WorkUnitValue = dto.WorkUnitValue,
                YearlyPto = dto.YearlyPto
            };
            var updated = await salaryPolicyRepository.UpdateSalaryPolicyAsync(entiy);
            return new SalaryPolicyViewDTO
            {
                EffectiveFrom = updated.EffectiveFrom,
                SalId = updated.SalId,
                IsActive = updated.IsActive,
                OverclockMultiplier = updated.OverclockMultiplier,
                SalaryPolicyName = updated.SalaryPolicyName,
                WorkUnitValue = updated.WorkUnitValue,
                YearlyPto = updated.YearlyPto
            };
        }
    }
}
