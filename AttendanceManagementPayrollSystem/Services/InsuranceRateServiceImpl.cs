using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public class InsuranceRateServiceImpl : InsuranceRateService
    {
        private readonly InsuranceRateRepository insuranceRateSetRepository;

        public InsuranceRateServiceImpl(InsuranceRateRepository insuranceRateSetRepository)
        {
            this.insuranceRateSetRepository = insuranceRateSetRepository;
        }

        public async Task<bool> AddInsuranceRate(InsuranceRateDTO dto)
        {
            if (dto == null) return false;
            InsuranceRate newInsuranceRate = new InsuranceRate
            {
                TypeName = dto.TypeName,
                EmployeeRate = dto.EmployeeRate,
                EmployerRate = dto.EmployerRate,
                CapRule = dto.CapRule,
                EffectiveFrom = dto.EffectiveFrom,
                EffectiveTo = dto.EffectiveTo,
                LawCode = dto.LawCode,
                IsActive = dto.IsActive,
                Category = dto.Category
            };
            await this.insuranceRateSetRepository.AddAsync(newInsuranceRate);
            return true;
        }

        public Task<List<int>> GetActiveIds()
        {
            return this.insuranceRateSetRepository.GetActiveIds();
        }

        public async Task<List<InsuranceRateDTO>> GetActiveInsuranceRateSetDTO()
        {
            return await this.insuranceRateSetRepository.GetActiveInsuranceRateDTO();
        }

        public async Task<List<InsuranceRateGroupDTO>> GetInsuranceRateGroupsAsync()
        {
            return await this.insuranceRateSetRepository.GetInsuranceRateGroupsAsync();
        }

        public async Task<List<InsuranceRateDTO>> GetInsuranceRateSetDTOs()
        {
            return await this.insuranceRateSetRepository.GetInsuranceRateDTOs();
        }

        public async Task<Dictionary<string, List<int>>> GetUpcomingInsuranceRateIds()
        {
            return await this.insuranceRateSetRepository.GetUpcomingInsuranceRateIds();
        }

        public async Task RemoveUpcomingInsuranceByIdAsync(int id)
        {
            var entity = await this.insuranceRateSetRepository.GetById(id);
            await this.insuranceRateSetRepository.RemoveAsync(entity);
        }

        public async Task<bool> UpdateActiveAsync(InsuranceRateDTO dto)
        {
            var oldEntity = await this.insuranceRateSetRepository.GetById(dto.RateSetId);
            if (oldEntity == null) return false;
            oldEntity.IsActive = false;
            await this.insuranceRateSetRepository.UpdateAsync(oldEntity);

            InsuranceRate newInsuranceRate = new InsuranceRate
            {
                TypeName = dto.TypeName,
                EmployeeRate = dto.EmployeeRate,
                EmployerRate = dto.EmployerRate,
                CapRule = dto.CapRule,
                EffectiveFrom = dto.EffectiveFrom,
                EffectiveTo = dto.EffectiveTo,
                LawCode = dto.LawCode,
                IsActive = true,
                Category = dto.Category
            };
            await this.insuranceRateSetRepository.AddAsync(newInsuranceRate);

            return true;


        }

        public async Task UpdateStatusAsync(int id, bool isActive)
        {
            var entity = await this.insuranceRateSetRepository.GetById(id);
            if (entity == null)
                throw new KeyNotFoundException();

            // Nếu kích hoạt bản ghi này → tắt các bản ghi khác cùng Category
            if (isActive)
            {
                var activeRates = await this.insuranceRateSetRepository.GetByCategoryAndActiveAsync(entity.Category);
                foreach (var rate in activeRates)
                {
                    if (rate.RateSetId != id)
                    {
                        rate.IsActive = false;
                        await this.insuranceRateSetRepository.UpdateAsync(rate);
                    }
                }
            }

            // Cập nhật trạng thái của bản ghi hiện tại
            entity.IsActive = isActive;
            await this.insuranceRateSetRepository.UpdateAsync(entity);
        }


        public async Task<bool> UpdateUpcomingAsync(InsuranceRateDTO dto)
        {
            var entity = await this.insuranceRateSetRepository.GetById(dto.RateSetId);
            if (entity == null) return false;
            entity.TypeName = dto.TypeName;
            entity.EmployeeRate = dto.EmployeeRate;
            entity.EmployerRate = dto.EmployerRate;
            entity.CapRule = dto.CapRule;
            entity.EffectiveFrom = dto.EffectiveFrom;
            entity.EffectiveTo = dto.EffectiveTo;
            entity.LawCode = dto.LawCode;
            entity.IsActive = dto.IsActive;
            entity.Category = dto.Category;
            await this.insuranceRateSetRepository.UpdateAsync(entity);
            return true;
        }
    }
}
