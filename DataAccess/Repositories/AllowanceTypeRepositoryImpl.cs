using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class AllowanceTypeRepositoryImpl : AllowanceTypeRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public AllowanceTypeRepositoryImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        public async Task<AllowanceType> AddAsync(AllowanceType allowanceType)
        {
            allowanceType.CreatedAt = DateTime.Now;
            _context.AllowanceTypes.Add(allowanceType);
            await _context.SaveChangesAsync();
            return allowanceType;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var allowanceType = await _context.AllowanceTypes.FindAsync(id);
            if (allowanceType == null)
                return false;

            _context.AllowanceTypes.Remove(allowanceType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<AllowanceType>> GetAllAsync()
        {
            return await _context.AllowanceTypes
                .OrderBy(at => at.TypeName)
                .ToListAsync();
        }

        public async Task<AllowanceType?> GetByIdAsync(int id)
        {
            return await _context.AllowanceTypes.FindAsync(id);
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<AllowanceType>> SearchAsync(string searchTerm)
        {
            return await _context.AllowanceTypes
                .Where(at => at.TypeName.Contains(searchTerm) || 
                            at.CalculationType.Contains(searchTerm))
                .OrderBy(at => at.TypeName)
                .ToListAsync();
        }

        public async Task<AllowanceType?> UpdateAsync(AllowanceType allowanceType)
        {
            var existingType = await _context.AllowanceTypes.FindAsync(allowanceType.TypeId);
            if (existingType == null)
                return null;

            existingType.TypeName = allowanceType.TypeName;
            existingType.CalculationType = allowanceType.CalculationType;
            existingType.Value = allowanceType.Value;
            existingType.EffectiveStartDate = allowanceType.EffectiveStartDate;

            await _context.SaveChangesAsync();
            return existingType;
        }
    }
}