using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class HolidayCalendarRepositoryImpl : BaseRepositoryImpl, HolidayCalendarRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _dbContext;

        public HolidayCalendarRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
            _dbContext = context;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách ngày nghỉ lễ (kèm các phòng ban liên quan).
        /// </summary>
        public async Task<IEnumerable<HolidayCalendar>> GetAllAsync()
        {
            return await _dbContext.HolidayCalendars
                .Include(h => h.DepartmentHolidayCalenders)
                    .ThenInclude(dh => dh.Dep)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ theo năm.
        /// </summary>
        public async Task<IEnumerable<HolidayCalendar>> GetByYearAsync(int year)
        {
            return await _dbContext.HolidayCalendars
                .Include(h => h.DepartmentHolidayCalenders)
                    .ThenInclude(dh => dh.Dep)
                .Where(h => h.PeriodYear.HasValue && h.PeriodYear.Value.Year == year)
                .OrderBy(h => h.HolidayName)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy chi tiết ngày nghỉ lễ theo ID.
        /// </summary>
        public async Task<HolidayCalendar?> GetByIdAsync(int holidayId)
        {
            return await _dbContext.HolidayCalendars
                .Include(h => h.DepartmentHolidayCalenders)
                    .ThenInclude(dh => dh.Dep)
                .FirstOrDefaultAsync(h => h.HolidayId == holidayId);
        }

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ áp dụng cho một phòng ban.
        /// </summary>
        public async Task<IEnumerable<HolidayCalendar>> GetByDepartmentAsync(int depId)
        {
            return await _dbContext.HolidayCalendars
                .Include(h => h.DepartmentHolidayCalenders)
                .Where(h => h.DepartmentHolidayCalenders.Any(dh => dh.DepId == depId))
                .OrderBy(h => h.HolidayName)
                .ToListAsync();
        }

        /// <summary>
        /// Thêm mới ngày nghỉ lễ.
        /// </summary>
        public async Task AddAsync(HolidayCalendar holiday)
        {
            holiday.CreatedAt = DateTime.Now;
            await _dbContext.HolidayCalendars.AddAsync(holiday);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật ngày nghỉ lễ.
        /// </summary>
        public async Task UpdateAsync(HolidayCalendar holiday)
        {
            var existing = await _dbContext.HolidayCalendars
                .FirstOrDefaultAsync(h => h.HolidayId == holiday.HolidayId);

            if (existing != null)
            {
                existing.HolidayName = holiday.HolidayName;
                existing.PeriodYear = holiday.PeriodYear;
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Xóa ngày nghỉ lễ (và các liên kết phòng ban).
        /// </summary>
        public async Task DeleteAsync(int holidayId)
        {
            var holiday = await _dbContext.HolidayCalendars
                .Include(h => h.DepartmentHolidayCalenders)
                .FirstOrDefaultAsync(h => h.HolidayId == holidayId);

            if (holiday != null)
            {
                _dbContext.DepartmentHolidayCalenders.RemoveRange(holiday.DepartmentHolidayCalenders);
                _dbContext.HolidayCalendars.Remove(holiday);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gán ngày nghỉ lễ cho một phòng ban (thêm vào bảng trung gian DepartmentHolidayCalender).
        /// </summary>
        public async Task AssignHolidayToDepartmentAsync(int holidayId, int depId, DateTime startDate, DateTime endDate)
        {
            var exists = await _dbContext.DepartmentHolidayCalenders
                .AnyAsync(x => x.HolidayId == holidayId && x.DepId == depId);

            if (!exists)
            {
                var link = new DepartmentHolidayCalender
                {
                    HolidayId = holidayId,
                    DepId = depId,
                    StartDate = startDate,
                    EndDate = endDate
                };

                await _dbContext.DepartmentHolidayCalenders.AddAsync(link);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gỡ ngày nghỉ lễ khỏi phòng ban.
        /// </summary>
        public async Task RemoveHolidayFromDepartmentAsync(int holidayId, int depId)
        {
            var record = await _dbContext.DepartmentHolidayCalenders
                .FirstOrDefaultAsync(x => x.HolidayId == holidayId && x.DepId == depId);

            if (record != null)
            {
                _dbContext.DepartmentHolidayCalenders.Remove(record);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
