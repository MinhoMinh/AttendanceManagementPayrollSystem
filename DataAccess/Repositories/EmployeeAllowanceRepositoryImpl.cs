using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class EmployeeAllowanceRepositoryImpl : EmployeeAllowanceRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public EmployeeAllowanceRepositoryImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        public async Task<EmployeeAllowance> AddAsync(EmployeeAllowance employeeAllowance)
        {
            employeeAllowance.CreatedAt = DateTime.Now;
            _context.Add(employeeAllowance);
            await _context.SaveChangesAsync();
            return employeeAllowance;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var employeeAllowance = await _context.Set<EmployeeAllowance>().FindAsync(id);
            if (employeeAllowance == null)
                return false;

            _context.Set<EmployeeAllowance>().Remove(employeeAllowance);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<EmployeeAllowance>> GetAllAsync()
        {
            return await _context.Set<EmployeeAllowance>()
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Include(ea => ea.CreatedByNavigation)
                .OrderByDescending(ea => ea.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeAllowance>> GetByAllowanceTypeAsync(int typeId)
        {
            return await _context.Set<EmployeeAllowance>()
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Include(ea => ea.CreatedByNavigation)
                .Where(ea => ea.TypeId == typeId)
                .OrderByDescending(ea => ea.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<EmployeeAllowance>> GetByEmployeeIdAsync(int empId)
        {
            return await _context.Set<EmployeeAllowance>()
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Include(ea => ea.CreatedByNavigation)
                .Where(ea => ea.EmpId == empId)
                .OrderByDescending(ea => ea.CreatedAt)
                .ToListAsync();
        }

        public async Task<EmployeeAllowance?> GetByIdAsync(int id)
        {
            return await _context.Set<EmployeeAllowance>()
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Include(ea => ea.CreatedByNavigation)
                .FirstOrDefaultAsync(ea => ea.Id == id);
        }

        public Task<int> SaveChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<EmployeeAllowance>> SearchAsync(string searchTerm, string status = null, DateOnly? startDate = null, DateOnly? endDate = null)
        {
            var query = _context.Set<EmployeeAllowance>()
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Include(ea => ea.CreatedByNavigation)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(ea =>
                    ea.Emp.EmpName.Contains(searchTerm) ||
                    ea.Emp.EmpName.Contains(searchTerm) ||
                    ea.Type.TypeName.Contains(searchTerm));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(ea => ea.Status == status);
            }

            if (startDate.HasValue)
            {
                query = query.Where(ea => ea.StartDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(ea => ea.EndDate <= endDate.Value);
            }

            return await query
                .OrderByDescending(ea => ea.CreatedAt)
                .ToListAsync();
        }

        public async Task<EmployeeAllowance?> UpdateAsync(EmployeeAllowance employeeAllowance)
        {
            var existingAllowance = await _context.Set<EmployeeAllowance>().FindAsync(employeeAllowance.Id);
            if (existingAllowance == null)
                return null;

            existingAllowance.CustomValue = employeeAllowance.CustomValue;
            existingAllowance.StartDate = employeeAllowance.StartDate;
            existingAllowance.EndDate = employeeAllowance.EndDate;
            existingAllowance.Status = employeeAllowance.Status;

            await _context.SaveChangesAsync();
            return existingAllowance;
        }
    }
}