using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class TaxRepositoryImpl : BaseRepositoryImpl, TaxRepository
    {
        public TaxRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        public async Task<Tax> AddTaxAsync(TaxEditDTO dto)
        {
            var oldTax = await _context.Taxes.FirstOrDefaultAsync(t => t.IsActive);
            if (oldTax != null)
            {
                oldTax.IsActive = false;
            }

            var newTax = new Tax
            {
                TaxName = dto.TaxName,
                EffectiveFrom = dto.EffectiveFrom,
                EffectiveTo = dto.EffectiveTo,
                IsActive = dto.IsActive,
                TaxBrackets = dto.TaxBrackets?.Select(b => new TaxBracket
                {
                    LowerBound = b.LowerBound,
                    UpperBound = b.UpperBound,
                    Rate = b.Rate
                }).ToList() ?? new List<TaxBracket>()
            };
            await _context.Taxes.AddAsync(newTax);
            await _context.SaveChangesAsync();
            return newTax;
        }

        public async Task<TaxDTO?> GetActiveTaxDTOs()
        {
            return await _context.Taxes.Where(t => t.IsActive == true)
                .Select(t => new TaxDTO
                {
                    EffectiveFrom = t.EffectiveFrom,
                    EffectiveTo = t.EffectiveTo,
                    TaxBrackets = t.TaxBrackets.Select(t => new TaxBracketDTO
                    {
                        TaxId = t.TaxId,
                        BracketId = t.BracketId,
                        LowerBound = t.LowerBound,
                        Rate = t.Rate,
                        UpperBound = t.UpperBound
                    }).ToList(),
                    TaxId = t.TaxId,
                    TaxName = t.TaxName,
                    IsActive=t.IsActive
                    
                }).FirstOrDefaultAsync();
        }

        public async Task<List<TaxDTO>> GetTaxDTOs()
        {
            var result= await _context.Taxes.Select(t => new TaxDTO
            {
                EffectiveFrom = t.EffectiveFrom,
                EffectiveTo = t.EffectiveTo,
                TaxBrackets = t.TaxBrackets.Select(t => new TaxBracketDTO
                {
                    TaxId = t.TaxId,
                    BracketId = t.BracketId,
                    LowerBound = t.LowerBound,
                    Rate = t.Rate,
                    UpperBound = t.UpperBound
                }).ToList(),
                TaxId = t.TaxId,
                TaxName = t.TaxName,
                IsActive=t.IsActive
            }).ToListAsync();
            result.Reverse();
            return result;
        }
    }
}
