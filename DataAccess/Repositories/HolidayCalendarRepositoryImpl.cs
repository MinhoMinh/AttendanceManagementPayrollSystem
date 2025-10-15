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
        /// Lấy toàn bộ danh sách ngày nghỉ lễ.
        /// </summary>
        public async Task<IEnumerable<HolidayCalendar>> GetAllAsync()
        {
            return await _dbContext.HolidayCalendars
                .Include(h => h.Deps)
                .OrderByDescending(h => h.StartDatetime)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ theo tháng và năm.
        /// </summary>
        public async Task<IEnumerable<HolidayCalendar>> GetByMonthAsync(int month, int year)
        {
            return await _dbContext.HolidayCalendars
                .Include(h => h.Deps)
                .Where(h => h.StartDatetime.Month == month && h.StartDatetime.Year == year)
                .OrderBy(h => h.StartDatetime)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy thông tin chi tiết ngày nghỉ lễ theo ID.
        /// </summary>
        public async Task<HolidayCalendar?> GetByIdAsync(int holidayId)
        {
            return await _dbContext.HolidayCalendars
                .Include(h => h.Deps)
                .FirstOrDefaultAsync(h => h.HolidayId == holidayId);
        }

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ áp dụng cho một phòng ban cụ thể.
        /// </summary>
        public async Task<IEnumerable<HolidayCalendar>> GetByDepartmentAsync(int depId)
        {
            return await _dbContext.HolidayCalendars
                .Include(h => h.Deps)
                .Where(h => h.Deps.Any(d => d.DepId == depId))
                .OrderBy(h => h.StartDatetime)
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
        /// Cập nhật thông tin ngày nghỉ lễ.
        /// </summary>
        public async Task UpdateAsync(HolidayCalendar holiday)
        {
            var existing = await _dbContext.HolidayCalendars
                .Include(h => h.Deps)
                .FirstOrDefaultAsync(h => h.HolidayId == holiday.HolidayId);

            if (existing != null)
            {
                existing.HolidayName = holiday.HolidayName;
                existing.StartDatetime = holiday.StartDatetime;
                existing.EndDatetime = holiday.EndDatetime;
                existing.CreatedAt = holiday.CreatedAt;

                _dbContext.HolidayCalendars.Update(existing);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Xóa ngày nghỉ lễ khỏi hệ thống.
        /// </summary>
        public async Task DeleteAsync(int holidayId)
        {
            var holiday = await _dbContext.HolidayCalendars
                .Include(h => h.Deps)
                .FirstOrDefaultAsync(h => h.HolidayId == holidayId);

            if (holiday != null)
            {
                // EF Core tự xóa liên kết many-to-many trong bảng trung gian
                holiday.Deps.Clear();
                _dbContext.HolidayCalendars.Remove(holiday);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gán ngày nghỉ lễ cho một phòng ban.
        /// </summary>
        public async Task AssignHolidayToDepartmentAsync(int holidayId, int depId)
        {
            var holiday = await _dbContext.HolidayCalendars
                .Include(h => h.Deps)
                .FirstOrDefaultAsync(h => h.HolidayId == holidayId);

            var department = await _dbContext.Departments
                .FirstOrDefaultAsync(d => d.DepId == depId);

            if (holiday != null && department != null && !holiday.Deps.Any(d => d.DepId == depId))
            {
                holiday.Deps.Add(department);
                await _dbContext.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Gỡ ngày nghỉ lễ khỏi một phòng ban.
        /// </summary>
        public async Task RemoveHolidayFromDepartmentAsync(int holidayId, int depId)
        {
            var holiday = await _dbContext.HolidayCalendars
                .Include(h => h.Deps)
                .FirstOrDefaultAsync(h => h.HolidayId == holidayId);

            if (holiday != null)
            {
                var department = holiday.Deps.FirstOrDefault(d => d.DepId == depId);
                if (department != null)
                {
                    holiday.Deps.Remove(department);
                    await _dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
