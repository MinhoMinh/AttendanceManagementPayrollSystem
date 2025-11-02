using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public class TaxServiceImpl : TaxService
    {
        private readonly TaxRepository taxRepository;

        public TaxServiceImpl(TaxRepository taxRepository)
        {
            this.taxRepository = taxRepository;
        }

        public async Task<TaxDTO> AddTaxAsync(TaxEditDTO dto)
        {
            var entity = await this.taxRepository.AddTaxAsync(dto);

            var result = new TaxDTO
            {
                TaxId = entity.TaxId,
                TaxName = entity.TaxName,
                EffectiveFrom = entity.EffectiveFrom,
                EffectiveTo = entity.EffectiveTo,
                IsActive = entity.IsActive,
                TaxBrackets = entity.TaxBrackets?.Select(b => new TaxBracketDTO
                {
                    BracketId = b.BracketId,
                    LowerBound = b.LowerBound,
                    UpperBound = b.UpperBound,
                    Rate = b.Rate
                }).ToList()
            };

            return result;
        }

        public async Task<TaxDTO?> GetActiveTaxDTOs()
        {
            return await this.taxRepository.GetActiveTaxDTOs();
        }

        public async Task<List<TaxDTO>> GetAllTaxDTOs()
        {
            return await this.taxRepository.GetTaxDTOs();
        }
    }
}
