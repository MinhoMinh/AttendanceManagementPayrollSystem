using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class SalaryPolicyRepositoryImpl : BaseRepositoryImpl, SalaryPolicyRepository
    {
        public SalaryPolicyRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        public async Task<SalaryPolicyViewDTO?> GetActiveSalaryPolicyAsync()
        {
            return await _context.SalaryPolicies.Where(s => s.IsActive == true)
                                                .Select(s => new SalaryPolicyViewDTO
                                                {
                                                    SalId=s.SalId,
                                                    IsActive=s.IsActive,
                                                    EffectiveFrom=s.EffectiveFrom,
                                                    OverclockMultiplier=s.OverclockMultiplier,
                                                    SalaryPolicyName=s.SalaryPolicyName,
                                                    WorkUnitValue=s.WorkUnitValue,
                                                    YearlyPto=s.YearlyPto
                                                }).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SalaryPolicyViewDTO>> GetAllAsync()
        {
            return await this._context.SalaryPolicies
                             .Select(s=>new SalaryPolicyViewDTO
                             {
                                 EffectiveFrom=s.EffectiveFrom,
                                 IsActive=s.IsActive,
                                 OverclockMultiplier=s.OverclockMultiplier,
                                 SalaryPolicyName=s.SalaryPolicyName,
                                 SalId=s.SalId,
                                 WorkUnitValue=s.WorkUnitValue,
                                 YearlyPto=s.YearlyPto
                             }).ToListAsync();
        }

        public async Task<List<SalaryPolicyViewDTO>> GetInactiveSalaryPolicyAsync()
        {
            return await _context.SalaryPolicies.Where(s => s.IsActive == false)
                                                .Select(s => new SalaryPolicyViewDTO
                                                {
                                                    SalId = s.SalId,
                                                    IsActive = s.IsActive,
                                                    EffectiveFrom = s.EffectiveFrom,
                                                    OverclockMultiplier = s.OverclockMultiplier,
                                                    SalaryPolicyName = s.SalaryPolicyName,
                                                    WorkUnitValue = s.WorkUnitValue,
                                                    YearlyPto = s.YearlyPto
                                                }).ToListAsync();
        }

        public async Task<SalaryPolicy> UpdateSalaryPolicyAsync(SalaryPolicy updatedSalaryPolicy)
        {
            var existing = await _context.SalaryPolicies
                                       .FirstOrDefaultAsync(s => s.SalId == updatedSalaryPolicy.SalId);
            if (existing == null)
            {
                throw new Exception("Policy not found");
            }
            existing.IsActive = false;
            _context.SalaryPolicies.Update(existing);

            var newPolicy = new SalaryPolicy
            {
                SalaryPolicyName = updatedSalaryPolicy.SalaryPolicyName,
                EffectiveFrom = updatedSalaryPolicy.EffectiveFrom,
                IsActive = true,
                OverclockMultiplier = updatedSalaryPolicy.OverclockMultiplier,
                WorkUnitValue = updatedSalaryPolicy.WorkUnitValue,
                YearlyPto = updatedSalaryPolicy.SalId
            };
            _context.SalaryPolicies.Add(newPolicy);
            await _context.SaveChangesAsync();
            return newPolicy;
        }
    }
}
