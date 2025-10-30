using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface HolidayCalendarRepository : BaseRepository
    {
        /// <summary>
        /// Lấy toàn bộ danh sách ngày nghỉ lễ.
        /// </summary>
        Task<IEnumerable<HolidayCalendar>> GetAllAsync();

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ theo năm (nếu muốn lọc theo PeriodYear).
        /// </summary>
        Task<IEnumerable<HolidayCalendar>> GetByYearAsync(int year);

        Task<IEnumerable<HolidayCalendarDTO>> GetByRangeAsync(DateTime start, DateTime end);

        /// <summary>
        /// Lấy thông tin chi tiết ngày nghỉ lễ theo ID.
        /// </summary>
        Task<HolidayCalendar?> GetByIdAsync(int holidayId);

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ áp dụng cho một phòng ban cụ thể.
        /// </summary>
        Task<IEnumerable<HolidayCalendar>> GetByDepartmentAsync(int depId);

        Task<IEnumerable<HolidayCalendarDTO>> GetByEmployeeAsync(int depId, DateTime start, DateTime end);



        /// <summary>
        /// Thêm mới ngày nghỉ lễ.
        /// </summary>
        Task AddAsync(HolidayCalendar holiday);

        /// <summary>
        /// Cập nhật thông tin ngày nghỉ lễ.
        /// </summary>
        Task UpdateAsync(HolidayCalendarDTO holiday);

        /// <summary>
        /// Xóa ngày nghỉ lễ khỏi hệ thống.
        /// </summary>
        Task DeleteAsync(int holidayId);

        /// <summary>
        /// Gán ngày nghỉ lễ cho một phòng ban (thêm bản ghi vào DepartmentHolidayCalender).
        /// Bổ sung startDate/endDate để lưu khoảng áp dụng cho phòng ban đó.
        /// </summary>
        Task AssignHolidayToDepartmentAsync(int holidayId, int depId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Xóa ngày nghỉ lễ khỏi một phòng ban.
        /// </summary>
        Task RemoveHolidayFromDepartmentAsync(int holidayId, int depId);

        Task<List<HolidayCalendarDTO>> GetFilteredHolidaysAsync(DateTime? start, DateTime? end, string? name);
    }
}
