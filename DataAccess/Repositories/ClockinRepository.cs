using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ClockinRepository : BaseRepository
    {

        Task<IEnumerable<Clockin>> GetByDateRangeAsync(int periodMonth, int periodYear);

        /// <summary>
        /// Lấy toàn bộ bản ghi chấm công của một nhân viên trong khoảng thời gian.
        /// </summary>
        Task<IEnumerable<Clockin>> GetByEmployeeAsync(int empId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Lấy bản ghi chấm công của một nhân viên trong một ngày cụ thể.
        /// </summary>
        Task<Clockin?> GetByEmployeeAndDateAsync(int empId, DateTime date);

        /// <summary>
        /// Tính tổng số giờ làm việc của nhân viên trong tháng.
        /// </summary>
        Task<decimal> GetTotalWorkUnitsAsync(int month, int year);

        /// <summary>
        /// Cập nhật thông tin chấm công (ví dụ: WorkUnits, ClockLog...).
        /// </summary>
        Task UpdateClockinAsync(Clockin clockin);

        /// <summary>
        /// Xóa bản ghi chấm công (nếu cần chỉnh sửa hoặc nhập sai).
        /// </summary>
        Task DeleteClockinAsync(int empId, DateTime date);


        Task<IEnumerable<Clockin>> GetByEmployeeAndMonthAsync(int empId, int month, int year);
    }
}
