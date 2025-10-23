using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class DepartmentRepositoryImpl : DepartmentRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public DepartmentRepositoryImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            return await _context.Departments
                .Include(d => d.Employees)                    // tùy chọn include nếu cần
                .Include(d => d.DepartmentHolidayCalenders)
                .Include(d => d.DepartmentWeeklyShifts)
                .OrderBy(d => d.DepName)
                .ToListAsync();
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments
                .Include(d => d.Employees)
                .Include(d => d.DepartmentHolidayCalenders)
                .Include(d => d.DepartmentWeeklyShifts)
                .FirstOrDefaultAsync(d => d.DepId == id);
        }
    }
}
