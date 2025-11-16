using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class AllowanceTypeRepositoryImpl : BaseRepositoryImpl, AllowanceTypeRepository
    {
        public AllowanceTypeRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        /// <summary>
        /// Lấy toàn bộ danh sách loại phụ cấp.
        /// </summary>
        public async Task<IEnumerable<AllowanceType>> GetAllAsync()
        {
            return await _context.AllowanceTypes
                .OrderBy(a => a.TypeId)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy thông tin loại phụ cấp theo ID.
        /// </summary>
        public async Task<AllowanceType?> GetByIdAsync(int typeId)
        {
            return await _context.AllowanceTypes
                .FirstOrDefaultAsync(a => a.TypeId == typeId);
        }

        /// <summary>
        /// Lấy danh sách loại phụ cấp có hiệu lực từ ngày chỉ định trở đi.
        /// </summary>
        public async Task<IEnumerable<AllowanceType>> GetEffectiveFromAsync(DateTime startDate)
        {
            var startDateOnly = DateOnly.FromDateTime(startDate);

            return await _context.AllowanceTypes
                .Where(a => a.EffectiveStartDate >= startDateOnly)
                .OrderBy(a => a.EffectiveStartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Thêm mới loại phụ cấp.
        /// </summary>
        public async Task AddAsync(AllowanceType allowanceType)
        {
            allowanceType.CreatedAt = DateTime.Now;
            _context.AllowanceTypes.Add(allowanceType);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật thông tin loại phụ cấp.
        /// </summary>
        public async Task UpdateAsync(AllowanceType allowanceType)
        {
            var existing = await _context.AllowanceTypes
                .FirstOrDefaultAsync(a => a.TypeId == allowanceType.TypeId);

            if (existing != null)
            {
                existing.TypeName = allowanceType.TypeName;
                existing.CalculationType = allowanceType.CalculationType;
                existing.Value = allowanceType.Value;
                existing.EffectiveStartDate = allowanceType.EffectiveStartDate;

                _context.AllowanceTypes.Update(existing);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Xóa loại phụ cấp khỏi hệ thống.
        /// </summary>
        public async Task DeleteAsync(int typeId)
        {
            var allowanceType = await _context.AllowanceTypes
                .FirstOrDefaultAsync(a => a.TypeId == typeId);

            if (allowanceType != null)
            {
                _context.AllowanceTypes.Remove(allowanceType);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Lấy danh sách loại phụ cấp theo kiểu tính toán (Fixed, Percentage...).
        /// </summary>
        public async Task<IEnumerable<AllowanceType>> GetByCalculationTypeAsync(string calculationType)
        {
            return await _context.AllowanceTypes
                .Where(a => a.CalculationType == calculationType)
                .ToListAsync();
        }
    }
}