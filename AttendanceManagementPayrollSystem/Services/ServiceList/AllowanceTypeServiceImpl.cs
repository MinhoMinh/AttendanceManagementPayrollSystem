using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public class AllowanceTypeServiceImpl : AllowanceTypeService
    {
        private readonly AllowanceTypeRepository _allowanceRepo;

        public AllowanceTypeServiceImpl(AllowanceTypeRepository allowanceRepo)
        {
            _allowanceRepo = allowanceRepo;
        }

        /// <summary>
        /// Lấy toàn bộ danh sách loại phụ cấp
        /// </summary>
        public async Task<IEnumerable<AllowanceTypeDTO>> GetAllAsync()
        {
            var types = await _allowanceRepo.GetAllAsync();
            return types.Select(a => MapToDTO(a));
        }

        /// <summary>
        /// Lấy thông tin chi tiết một loại phụ cấp theo ID
        /// </summary>
        public async Task<AllowanceTypeDTO?> GetByIdAsync(int typeId)
        {
            var type = await _allowanceRepo.GetByIdAsync(typeId);
            return type == null ? null : MapToDTO(type);
        }

        /// <summary>
        /// Thêm mới một loại phụ cấp
        /// </summary>
        public async Task<AllowanceTypeDTO> CreateAsync(AllowanceTypeDTO dto)
        {
            var entity = new AllowanceType
            {
                TypeName = dto.TypeName,
                CalculationType = dto.CalculationType,
                Value = dto.Value,
                EffectiveStartDate = dto.EffectiveStartDate
            };

            await _allowanceRepo.AddAsync(entity);

            // Gán lại ID và CreatedAt sau khi lưu
            dto.TypeId = entity.TypeId;
            dto.CreatedAt = entity.CreatedAt;

            return dto;
        }

        /// <summary>
        /// Mapping từ Entity sang DTO
        /// </summary>
        private AllowanceTypeDTO MapToDTO(AllowanceType entity)
        {
            return new AllowanceTypeDTO
            {
                TypeId = entity.TypeId,
                TypeName = entity.TypeName,
                CalculationType = entity.CalculationType,
                Value = entity.Value,
                EffectiveStartDate = entity.EffectiveStartDate,
                CreatedAt = entity.CreatedAt
            };
        }

        public async Task<AllowanceType> AddAllowanceTypeAsync(AllowanceType model)
        {
            await _allowanceRepo.AddAsync(model);
            return model;
        }

    }
}