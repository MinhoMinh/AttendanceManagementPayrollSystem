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
            return await _context.InsuranceRates.Where(i => i.EffectiveFrom <= now && i.IsActive == true)
                                          .Select(r => r.RateSetId)
                                          .ToListAsync();

        }

        public async Task<List<InsuranceRateDTO>> GetActiveInsuranceRateDTO()
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            var list = await _context.InsuranceRates.Where(i => i.EffectiveFrom <= now && i.IsActive == true)
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
                                                       TypeName = i.TypeName,
                                                       Category = i.Category
                                                   }).ToListAsync();
            return list;
        }

        public async Task<List<InsuranceRate>> GetByCategoryAndActiveAsync(string category)
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            return await _context.InsuranceRates
            .Where(r => r.Category == category && r.IsActive && r.EffectiveFrom<=now)
            .ToListAsync();
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
                LawCode = i.LawCode,
                Category = i.Category
            }).ToListAsync();

            list.Reverse();
            return list;
        }

        public async Task<List<InsuranceRateGroupDTO>> GetInsuranceRateGroupsAsync()
        {
            var now = DateOnly.FromDateTime(DateTime.Now);
            var all = await _context.InsuranceRates.ToListAsync();
            var groups = all
                .GroupBy(x => x.Category)
                .Select(g => new InsuranceRateGroupDTO
                {
                    Category = g.Key,
                    Inactive = g.Where(x => !x.IsActive&&x.EffectiveFrom<=now)
                            .Select(i => new InsuranceRateDTO
                            {
                                CapRule = i.CapRule,
                                EffectiveFrom = i.EffectiveFrom,
                                EffectiveTo = i.EffectiveTo,
                                EmployeeRate = i.EmployeeRate,
                                RateSetId = i.RateSetId,
                                EmployerRate = i.EmployerRate,
                                IsActive = i.IsActive,
                                TypeName = i.TypeName,
                                LawCode = i.LawCode,
                                Category = i.Category
                            })
                            .OrderByDescending(i => i.EffectiveFrom).ToList(),
                    Active = g.Where(x => x.IsActive && x.EffectiveFrom <= now)
                              .Select(i => new InsuranceRateDTO
                              {
                                  CapRule = i.CapRule,
                                  EffectiveFrom = i.EffectiveFrom,
                                  EffectiveTo = i.EffectiveTo,
                                  EmployeeRate = i.EmployeeRate,
                                  RateSetId = i.RateSetId,
                                  EmployerRate = i.EmployerRate,
                                  IsActive = i.IsActive,
                                  TypeName = i.TypeName,
                                  LawCode = i.LawCode,
                                  Category = i.Category
                              })
                            .OrderByDescending(i=>i.EffectiveFrom).ToList(),
                    Upcoming = g.Where(x => x.EffectiveFrom > now)
                               .Select(i => new InsuranceRateDTO
                               {
                                   CapRule = i.CapRule,
                                   EffectiveFrom = i.EffectiveFrom,
                                   EffectiveTo = i.EffectiveTo,
                                   EmployeeRate = i.EmployeeRate,
                                   RateSetId = i.RateSetId,
                                   EmployerRate = i.EmployerRate,
                                   IsActive = i.IsActive,
                                   TypeName = i.TypeName,
                                   LawCode = i.LawCode,
                                   Category = i.Category
                               })
                            .OrderByDescending(i => i.EffectiveFrom).ToList()
                }).ToList();
            return groups;
        }

        public async Task<Dictionary<string, List<int>>> GetUpcomingInsuranceRateIds()
        {
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            var query = await _context.InsuranceRates
                                    .Where(i => i.EffectiveFrom > now)
                                    .GroupBy(i => i.Category)
                                    .Select(g => new
                                    {
                                        Category = g.Key,
                                        RateIds = g.Select(x => x.RateSetId).ToList()
                                    })
                                    .ToListAsync();
            return query.ToDictionary(x => x.Category, x => x.RateIds);
        }

        public async Task RemoveAsync(InsuranceRate insuranceRate)
        {
            this._context.InsuranceRates.Remove(insuranceRate);
            await this._context.SaveChangesAsync();
        }

        public async Task UpdateAsync(InsuranceRate insuranceRate)
        {
            this._context.InsuranceRates.Update(insuranceRate);
            await this._context.SaveChangesAsync();
        }
    }
}
