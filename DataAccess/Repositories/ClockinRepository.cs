using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ClockinRepository : BaseRepository
    {

        Task<IEnumerable<Clockin>> GetByDateRangeAsync(int periodMonth, int periodYear);

        /// <summary>
        /// Lấy toàn bộ bản ghi chấm công của một nhân viên trong khoảng thời gian.
        /// </summary>
        Task<IEnumerable<Clockin>> GetByEmployeeAsync(int empId, DateTime startDate, int months);

        /// <summary>
        /// Lấy bản ghi chấm công của một nhân viên trong một ngày cụ thể.
        /// </summary>
        Task<Clockin?> GetByEmployeeAndDateAsync(int empId, DateTime date);

        /// <summary>
        /// Tính tổng số giờ làm việc của nhân viên trong tháng.
        /// </summary>
        Task<decimal> GetTotalWorkUnitsAsync(int month, int year);


        Task<Clockin?> GetByEmployeeAndMonthAsync(int empId, int month, int year);

        Task SaveClockinData(IEnumerable<ClockinDTO> clockins);
    }
}
