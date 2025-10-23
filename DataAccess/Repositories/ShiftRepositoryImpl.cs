using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class ShiftRepositoryImpl : BaseRepositoryImpl, ShiftRepository
    {

        public ShiftRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
            
        }

        public async Task<WeeklyShift?> GetWeeklyShift(int empId)
        {
            var shift = await _context.Employees
                .Where(e => e.EmpId == empId)
                .Select(e => e.Dep.DepartmentWeeklyShifts
                    .Where(dws => dws.IsActive)
                    .Select(dws => dws.Shift)
                    .FirstOrDefault())
                .FirstOrDefaultAsync();

            if (shift != null)
            {
                await _context.Entry(shift).Reference(s => s.MonDailyShift).LoadAsync();
                await _context.Entry(shift).Reference(s => s.TueDailyShift).LoadAsync();
                await _context.Entry(shift).Reference(s => s.WedDailyShift).LoadAsync();
                await _context.Entry(shift).Reference(s => s.ThuDailyShift).LoadAsync();
                await _context.Entry(shift).Reference(s => s.FriDailyShift).LoadAsync();
                await _context.Entry(shift).Reference(s => s.SatDailyShift).LoadAsync();
                await _context.Entry(shift).Reference(s => s.SunDailyShift).LoadAsync();
            }
            return shift;
        }

        public async Task<Dictionary<int, WeeklyShift?>> GetWeeklyShifts(IEnumerable<int> empIds)
        {
            var data = await _context.Employees
                .Where(e => empIds.Contains(e.EmpId))
                .Include(e => e.Dep)
                    .ThenInclude(d => d.DepartmentWeeklyShifts)
                        .ThenInclude(dws => dws.Shift)
                            .ThenInclude(s => s.MonDailyShift)
                .Include(e => e.Dep)
                    .ThenInclude(d => d.DepartmentWeeklyShifts)
                        .ThenInclude(dws => dws.Shift)
                            .ThenInclude(s => s.TueDailyShift)
                .Include(e => e.Dep)
                    .ThenInclude(d => d.DepartmentWeeklyShifts)
                        .ThenInclude(dws => dws.Shift)
                            .ThenInclude(s => s.WedDailyShift)
                .Include(e => e.Dep)
                    .ThenInclude(d => d.DepartmentWeeklyShifts)
                        .ThenInclude(dws => dws.Shift)
                            .ThenInclude(s => s.ThuDailyShift)
                .Include(e => e.Dep)
                    .ThenInclude(d => d.DepartmentWeeklyShifts)
                        .ThenInclude(dws => dws.Shift)
                            .ThenInclude(s => s.FriDailyShift)
                .Include(e => e.Dep)
                    .ThenInclude(d => d.DepartmentWeeklyShifts)
                        .ThenInclude(dws => dws.Shift)
                            .ThenInclude(s => s.SatDailyShift)
                .Include(e => e.Dep)
                    .ThenInclude(d => d.DepartmentWeeklyShifts)
                        .ThenInclude(dws => dws.Shift)
                            .ThenInclude(s => s.SunDailyShift)
                .ToListAsync();

            return data
                .Select(e => new
                {
                    e.EmpId,
                    Shift = e.Dep.DepartmentWeeklyShifts
                        .Where(dws => dws.IsActive)
                        .Select(dws => dws.Shift)
                        .FirstOrDefault()
                })
                .Where(x => x.Shift != null)
                .ToDictionary(x => x.EmpId, x => x.Shift);
        }


    }
}
