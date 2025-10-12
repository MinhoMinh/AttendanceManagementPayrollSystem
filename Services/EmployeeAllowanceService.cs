using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface EmployeeAllowanceService
    {
        /// <summary>
        /// Lấy toàn bộ danh sách phụ cấp của nhân viên (gồm thông tin nhân viên và loại phụ cấp).
        /// </summary>
        Task<IEnumerable<EmployeeAllowanceDTO>> GetAllAsync();

        /// <summary>
        /// Lấy danh sách nhân viên đang nhận một loại phụ cấp cụ thể.
        /// </summary>
        /// <param name="typeId">ID loại phụ cấp</param>
        Task<IEnumerable<EmployeeAllowanceDTO>> GetEmployeesByAllowanceTypeAsync(int typeId);

        /// <summary>
        /// Lấy danh sách phụ cấp theo mã nhân viên.
        /// </summary>
        /// <param name="empId">ID nhân viên</param>
        Task<IEnumerable<EmployeeAllowanceDTO>> GetByEmployeeIdAsync(int empId);

        /// <summary>
        /// Lấy thông tin chi tiết một bản ghi phụ cấp của nhân viên.
        /// </summary>
        /// <param name="id">ID bản ghi phụ cấp</param>
        Task<EmployeeAllowanceDTO?> GetByIdAsync(int id);

        /// <summary>
        /// Thêm mới một bản ghi phụ cấp cho nhân viên.
        /// </summary>
        Task AddAsync(EmployeeAllowanceCreateDTO dto);

        /// <summary>
        /// Cập nhật thông tin phụ cấp của nhân viên.
        /// </summary>
        Task UpdateAsync(EmployeeAllowanceCreateDTO dto);

        /// <summary>
        /// Xóa một bản ghi phụ cấp theo ID.
        /// </summary>
        Task DeleteAsync(int id);

        /// <summary>
        /// Lấy danh sách nhân viên có phụ cấp đang hoạt động tại ngày hiện tại.
        /// </summary>
        Task<IEnumerable<EmployeeAllowanceDTO>> GetActiveAllowancesAsync(DateOnly date);
    }
}
