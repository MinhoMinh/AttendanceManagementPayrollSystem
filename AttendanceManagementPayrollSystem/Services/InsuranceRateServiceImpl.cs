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

        public Task<List<int>> GetActiveIds()
        {
            return this.insuranceRateSetRepository.GetActiveIds();
        }

        public async Task<List<InsuranceRateDTO>> GetActiveInsuranceRateSetDTO()
        {
            return await this.insuranceRateSetRepository.GetActiveInsuranceRateDTO();
        }

        public async Task<List<InsuranceRateDTO>> GetInsuranceRateSetDTOs()
        {
            return await this.insuranceRateSetRepository.GetInsuranceRateDTOs();
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
            };
            await this.insuranceRateSetRepository.AddAsync(newInsuranceRate);

            return true;


        }

        public async Task<bool> UpdateInactiveAsync(InsuranceRateDTO dto)
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
            await this.insuranceRateSetRepository.UpdateAsync(entity);
            return true;
        }
    }
}
