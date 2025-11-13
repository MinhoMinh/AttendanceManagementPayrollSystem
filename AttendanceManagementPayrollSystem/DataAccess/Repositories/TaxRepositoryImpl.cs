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

        public async Task<bool> AddTaxAsync(TaxEditDTO dto)
        {
            if (dto == null) return false;

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
            return true;
        }

        public async Task<TaxDTO?> GetActiveTaxDTO()
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

        public async Task<TaxGroupDTO> GetTaxGroupAsync()
        {
            var all = await this._context.Taxes.Include(t=>t.TaxBrackets).ToListAsync();
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);

            var result = new TaxGroupDTO
            {
                Active = all.Where(t => t.IsActive && t.EffectiveFrom <= now)
                          .Select(t => new TaxDTO
                          {
                              TaxId = t.TaxId,
                              EffectiveFrom = t.EffectiveFrom,
                              IsActive = t.IsActive,
                              EffectiveTo = t.EffectiveTo,
                              TaxBrackets = t.TaxBrackets.Select(tb => new TaxBracketDTO
                              {
                                  BracketId = tb.BracketId,
                                  LowerBound = tb.LowerBound,
                                  TaxId = tb.TaxId,
                                  Rate = tb.Rate,
                                  UpperBound = tb.UpperBound
                              }).ToList(),
                              TaxName=t.TaxName
                          }).OrderByDescending(t=>t.EffectiveFrom).ToList(),
                Inactive=all.Where(t=>!t.IsActive&&t.EffectiveFrom<=now)
                            .Select(t=>new TaxDTO
                            {
                                TaxId = t.TaxId,
                                EffectiveFrom = t.EffectiveFrom,
                                IsActive = t.IsActive,
                                EffectiveTo = t.EffectiveTo,
                                TaxBrackets = t.TaxBrackets.Select(tb => new TaxBracketDTO
                                {
                                    BracketId = tb.BracketId,
                                    LowerBound = tb.LowerBound,
                                    TaxId = tb.TaxId,
                                    Rate = tb.Rate,
                                    UpperBound = tb.UpperBound
                                }).ToList(),
                                TaxName = t.TaxName
                            }).OrderByDescending(t => t.EffectiveFrom).ToList(),
                Upcoming= all.Where(t => !t.IsActive && t.EffectiveFrom > now)
                            .Select(t => new TaxDTO
                            {
                                TaxId = t.TaxId,
                                EffectiveFrom = t.EffectiveFrom,
                                IsActive = t.IsActive,
                                EffectiveTo = t.EffectiveTo,
                                TaxBrackets = t.TaxBrackets.Select(tb => new TaxBracketDTO
                                {
                                    BracketId = tb.BracketId,
                                    LowerBound = tb.LowerBound,
                                    TaxId = tb.TaxId,
                                    Rate = tb.Rate,
                                    UpperBound = tb.UpperBound
                                }).ToList(),
                                TaxName = t.TaxName
                            }).OrderByDescending(t => t.EffectiveFrom).ToList()
            };
            return result;
        }
        public async Task<Tax?> GetByIdAsync(int id)
        {
            return await _context.Taxes.FindAsync(id);
        }

        public async Task UpdateAsync(Tax tax)
        {
            _context.Taxes.Update(tax);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(Tax tax)
        {
            _context.Taxes.Remove(tax);
            await _context.SaveChangesAsync();
        }

        public async Task<Tax?> GetActiveTax()
        {
            return await _context.Taxes.Where(t => t.IsActive == true).FirstOrDefaultAsync();
        }

        //public async Task<Tax?> GetActiveTaxInTime(DateTime start, DateTime end)
        //{
        //    return await _context.Taxes
        //        .Where(t => t.IsActive
        //            && t.EffectiveFrom.ToDateTime(TimeOnly.MinValue) <= end
        //            && (
        //                t.EffectiveTo == null ||
        //                t.EffectiveTo.Value.ToDateTime(TimeOnly.MaxValue) >= start
        //            ))
        //        .OrderByDescending(t => t.TaxId)
        //        .FirstOrDefaultAsync();
        //}
        public async Task<Tax?> GetActiveTaxInTime(DateTime start, DateTime end)
        {
            return _context.Taxes
                .Where(t => t.IsActive)
                .AsEnumerable() // forces client evaluation
                .Where(t =>
                    t.EffectiveFrom.ToDateTime(TimeOnly.MinValue) <= end &&
                    (
                        t.EffectiveTo == null ||
                        t.EffectiveTo.Value.ToDateTime(TimeOnly.MaxValue) >= start
                    ))
                .OrderByDescending(t => t.TaxId)
                .FirstOrDefault();
        }

        public async Task RemoveBracketsAsync(int taxId)
        {
            var brackets = _context.TaxBrackets.Where(b => b.TaxId == taxId);
            _context.TaxBrackets.RemoveRange(brackets);
            await _context.SaveChangesAsync();
        }
    }
}
