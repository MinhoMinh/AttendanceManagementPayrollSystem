using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface DepartmentHolidayCalendarRepository : BaseRepository
    {
        // Thêm mới bản ghi gắn giữa Department và Holiday
        Task<DepartmentHolidayCalender> AddAsync(DepartmentHolidayCalender entity);

        // Lấy tất cả (có join để hiển thị thông tin DepName và HolidayName)
        Task<IEnumerable<DepartmentHolidayCalendarDTO>> GetAllAsync();

        // Lấy theo id
        Task<DepartmentHolidayCalendarDTO?> GetDtoByIdAsync(int id);

        // Cập nhật ngày bắt đầu/kết thúc
        Task UpdateAsync(DepartmentHolidayCalender entity);

        // Kiểm tra tồn tại bản ghi theo DepId & HolidayId
        Task<bool> ExistsAsync(int depId, int holidayId);
    }
}
