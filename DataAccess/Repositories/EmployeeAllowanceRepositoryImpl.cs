using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class EmployeeAllowanceRepositoryImpl : BaseRepositoryImpl, EmployeeAllowanceRepository
    {
        public EmployeeAllowanceRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        /// <summary>
        /// Lấy toàn bộ danh sách phụ cấp của nhân viên (bao gồm thông tin nhân viên và loại phụ cấp).
        /// </summary>
        public async Task<IEnumerable<EmployeeAllowance>> GetAllAsync()
        {
            return await _context.EmployeeAllowances
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Include(ea => ea.CreatedByNavigation)
                .OrderBy(ea => ea.Id)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy thông tin chi tiết một bản ghi phụ cấp theo ID.
        /// </summary>
        public async Task<EmployeeAllowance?> GetByIdAsync(int id)
        {
            return await _context.EmployeeAllowances
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Include(ea => ea.CreatedByNavigation)
                .FirstOrDefaultAsync(ea => ea.Id == id);
        }

        /// <summary>
        /// Lấy danh sách phụ cấp theo mã nhân viên (EmpId).
        /// </summary>
        public async Task<IEnumerable<EmployeeAllowance>> GetByEmployeeIdAsync(int empId)
        {
            return await _context.EmployeeAllowances
                .Include(ea => ea.Type)
                .Where(ea => ea.EmpId == empId)
                .OrderBy(ea => ea.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách phụ cấp theo loại phụ cấp (TypeId).
        /// </summary>
        public async Task<IEnumerable<EmployeeAllowance>> GetByTypeIdAsync(int typeId)
        {
            return await _context.EmployeeAllowances
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Where(ea => ea.TypeId == typeId)
                .OrderBy(ea => ea.EmpId)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy danh sách phụ cấp theo trạng thái (Active, Inactive, Pending...).
        /// </summary>
        public async Task<IEnumerable<EmployeeAllowance>> GetByStatusAsync(string status)
        {
            return await _context.EmployeeAllowances
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Where(ea => ea.Status == status)
                .OrderBy(ea => ea.StartDate)
                .ToListAsync();
        }

        /// <summary>
        /// Thêm mới một bản ghi phụ cấp cho nhân viên.
        /// </summary>
        public async Task AddAsync(EmployeeAllowance employeeAllowance)
        {
            employeeAllowance.CreatedAt = DateTime.Now;
            _context.EmployeeAllowances.Add(employeeAllowance);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật thông tin phụ cấp của nhân viên.
        /// </summary>
        public async Task UpdateAsync(EmployeeAllowance employeeAllowance)
        {
            var existing = await _context.EmployeeAllowances
                .FirstOrDefaultAsync(ea => ea.Id == employeeAllowance.Id);

            if (existing != null)
            {
                existing.EmpId = employeeAllowance.EmpId;
                existing.TypeId = employeeAllowance.TypeId;
                existing.CustomValue = employeeAllowance.CustomValue;
                existing.StartDate = employeeAllowance.StartDate;
                existing.EndDate = employeeAllowance.EndDate;
                existing.Status = employeeAllowance.Status;
                existing.CreatedBy = employeeAllowance.CreatedBy;
                // Không gán CreatedAt (giữ nguyên thời gian tạo)

                _context.EmployeeAllowances.Update(existing);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Xóa một bản ghi phụ cấp theo ID.
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            var entity = await _context.EmployeeAllowances
                .FirstOrDefaultAsync(ea => ea.Id == id);

            if (entity != null)
            {
                _context.EmployeeAllowances.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Lấy danh sách phụ cấp đang có hiệu lực trong ngày chỉ định.
        /// Điều kiện: StartDate <= date AND (EndDate == null OR EndDate >= date)
        /// </summary>
        public async Task<IEnumerable<EmployeeAllowance>> GetActiveAllowancesAsync(DateOnly date)
        {
            return await _context.EmployeeAllowances
                .Include(ea => ea.Emp)
                .Include(ea => ea.Type)
                .Where(ea => ea.StartDate <= date && (ea.EndDate == null || ea.EndDate >= date))
                .OrderBy(ea => ea.StartDate)
                .ToListAsync();
        }
    }
}
