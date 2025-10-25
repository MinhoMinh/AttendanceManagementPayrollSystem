using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
                .Include(c => c.ClockinComponents)
                .ToListAsync();
        }

        //

        public async Task<IEnumerable<Clockin>> GetByEmployeeAsync(int empId, DateTime startDate, int months)
        {
            var endDate = startDate.AddMonths(months).AddDays(-1); // inclusive period

            return await _context.Clockins
                .Where(c => c.EmpId == empId
                            && c.Date >= startDate.Date
                            && c.Date <= endDate.Date)
                .Include(c => c.ClockinComponents)
                .OrderBy(c => c.Date)
                .ToListAsync();
        }

        public async Task<Clockin?> GetByEmployeeAndDateAsync(int empId, DateTime date)
        {
            return await _context.Clockins
                .Include(c => c.ClockinComponents)
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

        public async Task<Clockin?> GetByEmployeeAndMonthAsync(int empId, int month, int year)
        {
            //var start = new DateTime(year, month, 1);
            //var end = start.AddMonths(1).AddDays(-1);

            return await _context.Clockins
                .Where(c => c.EmpId == empId && c.Date.Month == month && c.Date.Year == year)
                .Include(c => c.ClockinComponents)
                .FirstOrDefaultAsync();
        }

        public async Task SaveClockinData(IEnumerable<ClockinDTO> clockinDtos)
        {
            foreach (var dto in clockinDtos)
            {
                // Load existing clockin with components
                var existing = await _context.Clockins
                    .Include(c => c.ClockinComponents)
                    .FirstOrDefaultAsync(c => c.EmpId == dto.EmpId && c.Date.Year == dto.Date.Year &&
                                                                        c.Date.Month == dto.Date.Month);

                if (existing != null)
                {
                    // Update Clockin fields
                    existing.WorkUnits = dto.WorkUnits;
                    existing.ScheduledUnits = dto.ScheduledUnits;
                    existing.WorkHours = dto.WorkHours;
                    existing.ScheduledHours = dto.ScheduledHours;
                    existing.ClockLog = dto.ClockLog;
                    existing.WorkUnitBreakdown = dto.WorkUnitBreakdown;
                    foreach (var (compDto, existingComp) in
                    // Upsert ClockinComponents
                    from compDto in dto.Components
                    let existingComp = existing.ClockinComponents
                         .FirstOrDefault(c =>
                                c.CloId == existing.CloId &&
                                c.Shift == compDto.Shift &&
                                c.Date == compDto.Date)
                    select (compDto, existingComp))
                    {
                        if (existingComp != null)
                        {
                            // Update existing component
                            existingComp.Shift = compDto.Shift;
                            existingComp.Date = compDto.Date;
                            existingComp.CheckIn = compDto.CheckIn;
                            existingComp.CheckOut = compDto.CheckOut;
                            existingComp.ClockinLog = compDto.ClockinLog;
                            existingComp.Description = compDto.Description;
                            existingComp.WorkHours = compDto.WorkHours;
                            existingComp.ScheduledHours = compDto.ScheduledHours;
                            existingComp.WorkUnits = compDto.WorkUnits;
                            existingComp.ScheduledUnits = compDto.ScheduledUnits;
                        }
                        else
                        {
                            // Add new component
                            existing.ClockinComponents.Add(new ClockinComponent
                            {
                                CloId = existing.CloId,
                                Shift = compDto.Shift,
                                Date = compDto.Date,
                                CheckIn = compDto.CheckIn,
                                CheckOut = compDto.CheckOut,
                                ClockinLog = compDto.ClockinLog,
                                Description = compDto.Description,
                                WorkHours = compDto.WorkHours,
                                ScheduledHours = compDto.ScheduledHours,
                                WorkUnits = compDto.WorkUnits,
                                ScheduledUnits = compDto.ScheduledUnits
                            });
                        }
                    }
                }
                else
                {
                    // Add new Clockin with components
                    var newClockin = new Clockin
                    {
                        EmpId = dto.EmpId,
                        Date = dto.Date,
                        WorkUnits = dto.WorkUnits,
                        ScheduledUnits = dto.ScheduledUnits,
                        WorkHours= dto.WorkHours,
                        ScheduledHours= dto.ScheduledHours,
                        ClockLog = dto.ClockLog,
                        WorkUnitBreakdown = dto.WorkUnitBreakdown,
                        ClockinComponents = dto.Components?
                            .Select(c => new ClockinComponent
                            {
                                Shift = c.Shift,
                                Date = c.Date,
                                CheckIn = c.CheckIn,
                                CheckOut = c.CheckOut,
                                ClockinLog = c.ClockinLog,
                                Description = c.Description,
                                WorkHours = c.WorkHours,
                                ScheduledHours = c.ScheduledHours,
                                WorkUnits = c.WorkUnits,
                                ScheduledUnits = c.ScheduledUnits
                            }).ToList()
                    };

                    await _context.Clockins.AddAsync(newClockin);
                }

                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
        }
    }
}
