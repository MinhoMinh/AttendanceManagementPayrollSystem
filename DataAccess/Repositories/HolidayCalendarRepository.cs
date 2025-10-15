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
        /// Lấy danh sách ngày nghỉ lễ theo tháng và năm.
        /// </summary>
        Task<IEnumerable<HolidayCalendar>> GetByMonthAsync(int month, int year);

        /// <summary>
        /// Lấy thông tin chi tiết ngày nghỉ lễ theo ID.
        /// </summary>
        Task<HolidayCalendar?> GetByIdAsync(int holidayId);

        /// <summary>
        /// Lấy danh sách ngày nghỉ lễ áp dụng cho một phòng ban cụ thể.
        /// </summary>
        Task<IEnumerable<HolidayCalendar>> GetByDepartmentAsync(int depId);

        /// <summary>
        /// Thêm mới ngày nghỉ lễ.
        /// </summary>
        Task AddAsync(HolidayCalendar holiday);

        /// <summary>
        /// Cập nhật thông tin ngày nghỉ lễ.
        /// </summary>
        Task UpdateAsync(HolidayCalendar holiday);

        /// <summary>
        /// Xóa ngày nghỉ lễ khỏi hệ thống.
        /// </summary>
        Task DeleteAsync(int holidayId);

        /// <summary>
        /// Gán ngày nghỉ lễ cho một phòng ban.
        /// </summary>
        Task AssignHolidayToDepartmentAsync(int holidayId, int depId);

        /// <summary>
        /// Xóa ngày nghỉ lễ khỏi một phòng ban.
        /// </summary>
        Task RemoveHolidayFromDepartmentAsync(int holidayId, int depId);
    }
}
