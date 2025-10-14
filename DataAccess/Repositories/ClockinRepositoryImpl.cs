using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class ClockinRepositoryImpl : BaseRepositoryImpl, ClockinRepository
    {

        public ClockinRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Clockin>> GetByDateRangeAsync(int periodMonth, int periodYear)
        {
            var startDate = new DateTime(periodYear, periodMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await _context.Clockins
                .Where(c => c.Date >= startDate && c.Date <= endDate)
                .ToListAsync();
        }

        //

        public async Task<IEnumerable<Clockin>> GetByEmployeeAsync(int empId, DateTime startDate, DateTime endDate)
        {
            return await _context.Clockins
                .Where(c => c.EmpId == empId && c.Date >= startDate && c.Date <= endDate)
                .OrderBy(c => c.Date)
                .ToListAsync();
        }

        public async Task<Clockin?> GetByEmployeeAndDateAsync(int empId, DateTime date)
        {
            return await _context.Clockins
                .FirstOrDefaultAsync(c => c.EmpId == empId && c.Date.Date == date.Date);
        }

        public async Task<decimal> GetTotalWorkUnitsAsync(int month, int year)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await _context.Clockins
                .Where(c => c.Date >= startDate && c.Date <= endDate)
                .SumAsync(c => c.WorkUnits ?? 0);
        }

        public async Task UpdateClockinAsync(Clockin clockin)
        {
            var existing = await _context.Clockins
                .FirstOrDefaultAsync(c => c.EmpId == clockin.EmpId && c.Date.Date == clockin.Date.Date);

            if (existing != null)
            {
                existing.ClockLog = clockin.ClockLog;
                existing.WorkUnits = clockin.WorkUnits;
                existing.WorkUnitBreakdown = clockin.WorkUnitBreakdown;
                existing.ScheduledUnits = clockin.ScheduledUnits;

                _context.Clockins.Update(existing);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteClockinAsync(int empId, DateTime date)
        {
            var clockin = await _context.Clockins
                .FirstOrDefaultAsync(c => c.EmpId == empId && c.Date.Date == date.Date);

            if (clockin != null)
            {
                _context.Clockins.Remove(clockin);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Clockin>> GetByEmployeeAndMonthAsync(int empId, int month, int year)
        {
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1).AddDays(-1);

            return await _context.Clockins
                .Where(c => c.EmpId == empId && c.Date >= start && c.Date <= end)
                .ToListAsync();
        }
    }
}
