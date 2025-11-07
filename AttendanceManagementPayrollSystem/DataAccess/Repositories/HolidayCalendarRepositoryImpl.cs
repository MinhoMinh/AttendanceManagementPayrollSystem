using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.DTO;
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

        public async Task<IEnumerable<HolidayCalendarDTO>> GetByRangeAsync(DateTime start, DateTime end)
        {
            return await _dbContext.HolidayCalendars
        .Include(h => h.DepartmentHolidayCalenders)
            .ThenInclude(dh => dh.Dep)
        .Where(h => h.DepartmentHolidayCalenders.Any(dh =>
            dh.StartDate <= end &&
            dh.EndDate >= start))
        .OrderBy(h => h.HolidayName)
        .Select(h => new HolidayCalendarDTO
        {
            HolidayId = h.HolidayId,
            HolidayName = h.HolidayName,
            PeriodYear = h.PeriodYear,
            CreatedAt = h.CreatedAt,
            DepartmentHolidays = h.DepartmentHolidayCalenders
                .Where(dh => dh.StartDate <= end && dh.EndDate >= start)
                .Select(dh => new DepartmentHolidayCalendarDTO
                {
                    DepHolidayCalendarId = dh.DepHolidayCalendarId,
                    DepId = dh.DepId,
                    HolidayId = dh.HolidayId,
                    StartDate = dh.StartDate,
                    EndDate = dh.EndDate,
                    DepName = dh.Dep.DepName,
                    HolidayName = h.HolidayName
                }).ToList()
        })
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

        public async Task<IEnumerable<HolidayCalendarDTO>> GetByEmployeeAsync(int empId, DateTime start, DateTime end)
        {
            return await _dbContext.HolidayCalendars
                .Include(h => h.DepartmentHolidayCalenders)
                    .ThenInclude(dh => dh.Dep)
                .Where(h => h.DepartmentHolidayCalenders.Any(dh =>
                    dh.Dep.Employees.Any(e => e.EmpId == empId) &&
                    dh.StartDate <= end &&
                    dh.EndDate >= start))
                .OrderBy(h => h.HolidayName)
                .Select(h => new HolidayCalendarDTO
                {
                    HolidayId = h.HolidayId,
                    HolidayName = h.HolidayName,
                    PeriodYear = h.PeriodYear,
                    CreatedAt = h.CreatedAt,
                    DepartmentHolidays = h.DepartmentHolidayCalenders
                        .Where(dh =>
                            dh.StartDate <= end &&
                            dh.EndDate >= start &&
                            dh.Dep.Employees.Any(e => e.EmpId == empId)) // filter here
                        .Select(dh => new DepartmentHolidayCalendarDTO
                        {
                            DepHolidayCalendarId = dh.DepHolidayCalendarId,
                            DepId = dh.DepId,
                            HolidayId = dh.HolidayId,
                            StartDate = dh.StartDate,
                            EndDate = dh.EndDate,
                            DepName = dh.Dep.DepName,
                            HolidayName = h.HolidayName
                        })
                        .ToList()
                })
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
        public async Task UpdateAsync(HolidayCalendarDTO dto)
        {
            // Load existing entity including children
            var entity = await _context.HolidayCalendars
                .Include(h => h.DepartmentHolidayCalenders)
                .FirstOrDefaultAsync(h => h.HolidayId == dto.HolidayId);

            if (entity == null)
                throw new InvalidOperationException($"Holiday with ID {dto.HolidayId} not found");

            // Update holiday scalar properties
            entity.HolidayName = dto.HolidayName;
            entity.PeriodYear = dto.PeriodYear;
            entity.CreatedAt = dto.CreatedAt;

            // Sync DepartmentHolidayCalenders
            var dtoDeps = dto.DepartmentHolidays ?? new List<DepartmentHolidayCalendarDTO>();

            // Remove deleted
            var toRemove = entity.DepartmentHolidayCalenders
                .Where(e => !dtoDeps.Any(d => d.DepHolidayCalendarId == e.DepHolidayCalendarId))
                .ToList();
            foreach (var remove in toRemove)
                _context.DepartmentHolidayCalenders.Remove(remove);

            // Update existing and add new
            foreach (var depDto in dtoDeps)
            {
                var existing = entity.DepartmentHolidayCalenders
                    .FirstOrDefault(e => e.DepId == depDto.DepId && e.HolidayId == depDto.HolidayId);

                if (existing != null)
                {
                    existing.DepId = depDto.DepId;
                    existing.StartDate = depDto.StartDate;
                    existing.EndDate = depDto.EndDate;
                }
                else
                {
                    entity.DepartmentHolidayCalenders.Add(new DepartmentHolidayCalender
                    {
                        DepId = depDto.DepId,
                        HolidayId = dto.HolidayId,
                        StartDate = depDto.StartDate,
                        EndDate = depDto.EndDate
                    });
                }
            }

            await _context.SaveChangesAsync();
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

        public async Task<List<HolidayCalendarDTO>> GetFilteredHolidaysAsync(DateTime? start, DateTime? end, string? name)
        {
            // Base holidays query with filters
            var holidaysQuery = _context.HolidayCalendars.AsQueryable();

            if (start.HasValue)
                holidaysQuery = holidaysQuery.Where(h => h.PeriodYear >= start.Value);

            if (end.HasValue)
                holidaysQuery = holidaysQuery.Where(h => h.PeriodYear <= end.Value);

            if (!string.IsNullOrWhiteSpace(name))
                holidaysQuery = holidaysQuery.Where(h => h.HolidayName.Contains(name));

            // Project holidays into DTOs and populate DepartmentHolidays via join
            var resultQuery =
                from h in holidaysQuery
                orderby h.PeriodYear descending
                select new HolidayCalendarDTO
                {
                    HolidayId = h.HolidayId,
                    HolidayName = h.HolidayName,
                    PeriodYear = h.PeriodYear,
                    CreatedAt = h.CreatedAt,

                    DepartmentHolidays =
                        (from dh in _context.DepartmentHolidayCalenders
                         where dh.HolidayId == h.HolidayId
                         join dep in _context.Departments on dh.DepId equals dep.DepId
                         // join to holiday table only if you need holiday name from that table; here h.HolidayName is available
                         select new DepartmentHolidayCalendarDTO
                         {
                             DepHolidayCalendarId = dh.DepHolidayCalendarId,
                             DepId = dh.DepId,
                             HolidayId = dh.HolidayId,
                             StartDate = dh.StartDate,
                             EndDate = dh.EndDate,
                             DepName = dep.DepName,
                             HolidayName = h.HolidayName
                         }).ToList()
                };

            return await resultQuery.ToListAsync();
        }
    }
}
