using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface AllowanceTypeRepository : BaseRepository
    {
        /// <summary>
        /// Lấy toàn bộ danh sách loại phụ cấp hiện có.
        /// </summary>
        Task<IEnumerable<AllowanceType>> GetAllAsync();

        /// <summary>
        /// Lấy thông tin một loại phụ cấp theo ID.
        /// </summary>
        Task<AllowanceType?> GetByIdAsync(int typeId);

        /// <summary>
        /// Lấy danh sách phụ cấp có hiệu lực từ ngày cụ thể trở đi.
        /// </summary>
        Task<IEnumerable<AllowanceType>> GetEffectiveFromAsync(DateTime startDate);

        /// <summary>
        /// Thêm mới một loại phụ cấp.
        /// </summary>
        Task AddAsync(AllowanceType allowanceType);

        /// <summary>
        /// Cập nhật thông tin loại phụ cấp (tên, giá trị, ngày hiệu lực...).
        /// </summary>
        Task UpdateAsync(AllowanceType allowanceType);

        /// <summary>
        /// Xóa một loại phụ cấp khỏi hệ thống.
        /// </summary>
        Task DeleteAsync(int typeId);

        /// <summary>
        /// Lấy danh sách loại phụ cấp theo kiểu tính toán (ví dụ: Fixed, Percentage...).
        /// </summary>
        Task<IEnumerable<AllowanceType>> GetByCalculationTypeAsync(string calculationType);
    }
}
