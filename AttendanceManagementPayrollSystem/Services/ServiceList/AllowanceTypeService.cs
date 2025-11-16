using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface AllowanceTypeService
    {
        /// <summary>
        /// Lấy danh sách tất cả loại phụ cấp.
        /// </summary>
        Task<IEnumerable<AllowanceTypeDTO>> GetAllAsync();

        /// <summary>
        /// Lấy thông tin chi tiết một loại phụ cấp theo ID.
        /// </summary>
        Task<AllowanceTypeDTO?> GetByIdAsync(int typeId);

        /// <summary>
        /// Thêm mới một loại phụ cấp.
        /// </summary>
        Task<AllowanceTypeDTO> CreateAsync(AllowanceTypeDTO dto);

        Task<AllowanceType> AddAllowanceTypeAsync(AllowanceType model);
    }
}