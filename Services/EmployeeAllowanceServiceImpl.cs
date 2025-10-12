using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public class EmployeeAllowanceServiceImpl : EmployeeAllowanceService
    {
        private readonly EmployeeAllowanceRepository _employeeAllowanceRepo;

        public EmployeeAllowanceServiceImpl(EmployeeAllowanceRepository employeeAllowanceRepo)
        {
            _employeeAllowanceRepo = employeeAllowanceRepo;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách phụ cấp nhân viên
        /// </summary>
        public async Task<IEnumerable<EmployeeAllowanceDTO>> GetAllAsync()
        {
            var list = await _employeeAllowanceRepo.GetAllAsync();
            return list.Select(MapToDTO);
        }

        /// <summary>
        /// Lấy danh sách nhân viên đang nhận một loại phụ cấp cụ thể
        /// </summary>
        public async Task<IEnumerable<EmployeeAllowanceDTO>> GetEmployeesByAllowanceTypeAsync(int typeId)
        {
            var list = await _employeeAllowanceRepo.GetByTypeIdAsync(typeId);
            return list.Select(MapToDTO);
        }

        /// <summary>
        /// Lấy danh sách phụ cấp theo nhân viên
        /// </summary>
        public async Task<IEnumerable<EmployeeAllowanceDTO>> GetByEmployeeIdAsync(int empId)
        {
            var list = await _employeeAllowanceRepo.GetByEmployeeIdAsync(empId);
            return list.Select(MapToDTO);
        }

        /// <summary>
        /// Lấy thông tin chi tiết phụ cấp theo ID
        /// </summary>
        public async Task<EmployeeAllowanceDTO?> GetByIdAsync(int id)
        {
            var record = await _employeeAllowanceRepo.GetByIdAsync(id);
            return record == null ? null : MapToDTO(record);
        }

        /// <summary>
        /// Thêm mới phụ cấp cho nhân viên
        /// </summary>
        public async Task AddAsync(EmployeeAllowanceCreateDTO dto)
        {
            var entity = new EmployeeAllowance
            {
                EmpId = dto.EmpId,
                TypeId = dto.TypeId,
                CustomValue = dto.CustomValue,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = dto.Status,
                CreatedBy = 3
            };

            await _employeeAllowanceRepo.AddAsync(entity);

        }

        /// <summary>
        /// Cập nhật phụ cấp nhân viên
        /// </summary>
        public async Task UpdateAsync(EmployeeAllowanceCreateDTO dto)
        {
            var entity = await _employeeAllowanceRepo.GetByIdAsync(dto.Id);
            if (entity == null)
                throw new Exception($"EmployeeAllowance with ID {dto.Id} not found");

            entity.EmpId = dto.EmpId;
            entity.TypeId = dto.TypeId;
            entity.CustomValue = dto.CustomValue;
            entity.StartDate = dto.StartDate;
            entity.EndDate = dto.EndDate;
            entity.Status = dto.Status;
            entity.CreatedBy = dto.CreatedBy;

            await _employeeAllowanceRepo.UpdateAsync(entity);
        }


        /// <summary>
        /// Xóa phụ cấp nhân viên theo ID
        /// </summary>
        public async Task DeleteAsync(int id)
        {
            await _employeeAllowanceRepo.DeleteAsync(id);
        }

        /// <summary>
        /// Lấy danh sách phụ cấp đang có hiệu lực trong ngày hiện tại
        /// </summary>
        public async Task<IEnumerable<EmployeeAllowanceDTO>> GetActiveAllowancesAsync(DateOnly date)
        {
            var list = await _employeeAllowanceRepo.GetActiveAllowancesAsync(date);
            return list.Select(MapToDTO);
        }

        /// <summary>
        /// Mapping từ Entity sang DTO
        /// </summary>
        private EmployeeAllowanceDTO MapToDTO(EmployeeAllowance entity)
        {
            return new EmployeeAllowanceDTO
            {
                Id = entity.Id,
                EmpId = entity.EmpId,
                EmpName = entity.Emp?.EmpName,
                TypeId = entity.TypeId,
                TypeName = entity.Type?.TypeName,
                CustomValue = entity.CustomValue,
                StartDate = entity.StartDate,
                EndDate = entity.EndDate,
                Status = entity.Status,
                CreatedBy = entity.CreatedBy,
                CreatedByName = entity.CreatedByNavigation?.EmpName, // <— thêm dòng này
                CreatedAt = entity.CreatedAt
            };
        }

    }
}
