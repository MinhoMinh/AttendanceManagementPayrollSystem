using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class InsuranceRateRepositoryImpl : BaseRepositoryImpl, InsuranceRateRepository
    {
        public InsuranceRateRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        public async Task AddAsync(InsuranceRate entity)
        {
            this._context.InsuranceRates.Add(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task<List<int>> GetActiveIds()
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            return await _context.InsuranceRates.Where(i => i.EffectiveFrom <= now && (i.EffectiveTo == null || i.EffectiveTo >= now) && i.IsActive == true)
                                          .Select(r => r.RateSetId)
                                          .ToListAsync();

        }

        public async Task<List<InsuranceRateDTO>> GetActiveInsuranceRateDTO()
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            var list = await _context.InsuranceRates.Where(i => i.EffectiveFrom <= now && (i.EffectiveTo == null || i.EffectiveTo >= now) && i.IsActive == true)
                                                   .Select(i => new InsuranceRateDTO
                                                   {
                                                       CapRule = i.CapRule,
                                                       EffectiveFrom = i.EffectiveFrom,
                                                       EffectiveTo = i.EffectiveTo,
                                                       EmployeeRate = i.EmployeeRate,
                                                       RateSetId = i.RateSetId,
                                                       EmployerRate = i.EmployerRate,
                                                       IsActive = i.IsActive,
                                                       LawCode = i.LawCode,
                                                       TypeName = i.TypeName
                                                   }).ToListAsync();
            list.Reverse();
            return list;
        }

        public async Task<InsuranceRate?> GetById(int id)
        {
            return await _context.InsuranceRates.FindAsync(id);
        }

        public async Task<List<InsuranceRateDTO>> GetInsuranceRateDTOs()
        {
            var list = await _context.InsuranceRates.Select(i => new InsuranceRateDTO
            {
                CapRule = i.CapRule,
                EffectiveFrom = i.EffectiveFrom,
                EffectiveTo = i.EffectiveTo,
                EmployeeRate = i.EmployeeRate,
                RateSetId = i.RateSetId,
                EmployerRate = i.EmployerRate,
                IsActive = i.IsActive,
                TypeName = i.TypeName,
                LawCode = i.LawCode
            }).ToListAsync();

            list.Reverse();
            return list;
        }

        public async Task UpdateAsync(InsuranceRate insuranceRate)
        {
            this._context.InsuranceRates.Update(insuranceRate);
            await this._context.SaveChangesAsync();
        }
    }
}
