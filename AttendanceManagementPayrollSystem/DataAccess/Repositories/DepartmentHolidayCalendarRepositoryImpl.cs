using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class DepartmentHolidayCalendarRepositoryImpl : BaseRepositoryImpl, DepartmentHolidayCalendarRepository
    {
        public DepartmentHolidayCalendarRepositoryImpl(AttendanceManagementPayrollSystemContext context)
            : base(context)
        {
        }

        public async Task<DepartmentHolidayCalender> AddAsync(DepartmentHolidayCalender entity)
        {
            _context.DepartmentHolidayCalenders.Add(entity);
            await SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<DepartmentHolidayCalendarDTO>> GetAllAsync()
        {
            var query = from dh in _context.DepartmentHolidayCalenders
                        join d in _context.Departments on dh.DepId equals d.DepId
                        join h in _context.HolidayCalendars on dh.HolidayId equals h.HolidayId
                        select new DepartmentHolidayCalendarDTO
                        {
                            DepHolidayCalendarId = dh.DepHolidayCalendarId,
                            DepId = d.DepId,
                            HolidayId = h.HolidayId,
                            StartDate = dh.StartDate,
                            EndDate = dh.EndDate,
                            DepName = d.DepName,
                            HolidayName = h.HolidayName
                        };

            return await query.OrderByDescending(x => x.StartDate).ToListAsync();
        }

        public async Task<DepartmentHolidayCalendarDTO?> GetDtoByIdAsync(int id)
        {
            return await (from dh in _context.DepartmentHolidayCalenders
                          join d in _context.Departments on dh.DepId equals d.DepId
                          join h in _context.HolidayCalendars on dh.HolidayId equals h.HolidayId
                          where dh.DepHolidayCalendarId == id
                          select new DepartmentHolidayCalendarDTO
                          {
                              DepHolidayCalendarId = dh.DepHolidayCalendarId,
                              DepId = d.DepId,
                              HolidayId = h.HolidayId,
                              StartDate = dh.StartDate,
                              EndDate = dh.EndDate,
                              DepName = d.DepName,
                              HolidayName = h.HolidayName
                          }).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(DepartmentHolidayCalender entity)
        {
            _context.DepartmentHolidayCalenders.Update(entity);
            await SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int depId, int holidayId)
        {
            return await _context.DepartmentHolidayCalenders
                .AnyAsync(x => x.DepId == depId && x.HolidayId == holidayId);
        }
    }
}
