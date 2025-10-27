using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class EmployeeDependentRepositoryImpl : EmployeeDependentRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public EmployeeDependentRepositoryImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeDependent>> GetAllAsync()
        {
            return await _context.EmployeeDependents
                .Include(ed => ed.Employee)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeDependent>> GetByEmployeeIdAsync(int employeeId)
        {
            return await _context.EmployeeDependents
                .Include(ed => ed.Employee)
                .Where(ed => ed.EmployeeId == employeeId)
                .ToListAsync();
        }

        public async Task<EmployeeDependent?> GetByIdAsync(int id)
        {
            return await _context.EmployeeDependents
                .Include(ed => ed.Employee)
                .FirstOrDefaultAsync(ed => ed.DependentId == id);
        }

        public async Task<EmployeeDependent> AddAsync(EmployeeDependent dependent, int createdBy)
        {
            dependent.CreatedBy = createdBy;
            dependent.CreatedDate = DateTime.Now;
            
            _context.EmployeeDependents.Add(dependent);
            await _context.SaveChangesAsync();
            
            // Load the Employee navigation property
            await _context.Entry(dependent)
                .Reference(ed => ed.Employee)
                .LoadAsync();
            
            return dependent;
        }

        public async Task<EmployeeDependent?> UpdateAsync(EmployeeDependent dependent)
        {
            var existing = await _context.EmployeeDependents.FindAsync(dependent.DependentId);
            if (existing == null)
            {
                return null;
            }

            existing.FullName = dependent.FullName;
            existing.Relationship = dependent.Relationship;
            existing.DateOfBirth = dependent.DateOfBirth;
            existing.Gender = dependent.Gender;
            existing.NationalId = dependent.NationalId;
            existing.IsTaxDependent = dependent.IsTaxDependent;
            existing.EffectiveStartDate = dependent.EffectiveStartDate;
            existing.EffectiveEndDate = dependent.EffectiveEndDate;
            existing.Proof = dependent.Proof;

            await _context.SaveChangesAsync();

            // Load the Employee navigation property
            await _context.Entry(existing)
                .Reference(ed => ed.Employee)
                .LoadAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var dependent = await _context.EmployeeDependents.FindAsync(id);
            if (dependent == null)
            {
                return false;
            }

            _context.EmployeeDependents.Remove(dependent);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
