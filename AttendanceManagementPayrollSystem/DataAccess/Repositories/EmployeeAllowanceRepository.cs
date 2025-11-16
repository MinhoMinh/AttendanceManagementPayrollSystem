using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface EmployeeAllowanceRepository : BaseRepository
    {
        /// <summary>
        /// Lấy toàn bộ danh sách phụ cấp của nhân viên (bao gồm thông tin nhân viên và loại phụ cấp).
        /// </summary>
        Task<IEnumerable<EmployeeAllowance>> GetAllAsync();

        /// <summary>
        /// Lấy thông tin chi tiết một bản ghi phụ cấp theo ID.
        /// </summary>
        Task<EmployeeAllowance?> GetByIdAsync(int id);

        /// <summary>
        /// Lấy danh sách phụ cấp theo mã nhân viên (EmpId).
        /// </summary>
        Task<IEnumerable<EmployeeAllowance>> GetByEmployeeIdAsync(int empId);

        /// <summary>
        /// Lấy danh sách phụ cấp theo loại phụ cấp (TypeId).
        /// </summary>
        Task<IEnumerable<EmployeeAllowance>> GetByTypeIdAsync(int typeId);

        /// <summary>
        /// Lấy danh sách phụ cấp theo trạng thái (Active, Inactive, Pending...).
        /// </summary>
        Task<IEnumerable<EmployeeAllowance>> GetByStatusAsync(string status);

        /// <summary>
        /// Thêm mới một bản ghi phụ cấp cho nhân viên.
        /// </summary>
        Task AddAsync(EmployeeAllowance employeeAllowance);

        /// <summary>
        /// Cập nhật thông tin phụ cấp của nhân viên.
        /// </summary>
        Task UpdateAsync(EmployeeAllowance employeeAllowance);

        /// <summary>
        /// Xóa một bản ghi phụ cấp theo ID.
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Lấy danh sách phụ cấp đang có hiệu lực trong ngày hiện tại.
        /// </summary>
        Task<IEnumerable<EmployeeAllowance>> GetActiveAllowancesAsync(DateOnly date);
    }
}