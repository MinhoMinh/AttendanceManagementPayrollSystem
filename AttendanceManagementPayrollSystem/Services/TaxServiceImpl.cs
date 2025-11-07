using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public class TaxServiceImpl : TaxService
    {
        private readonly TaxRepository taxRepository;

        public TaxServiceImpl(TaxRepository taxRepository)
        {
            this.taxRepository = taxRepository;
        }

        public async Task<bool> AddTaxAsync(TaxEditDTO dto)
        {
            var entity = await this.taxRepository.AddTaxAsync(dto);

            return true;
        }

        public async Task<TaxDTO?> GetActiveTaxDTO()
        {
            return await this.taxRepository.GetActiveTaxDTO();
        }

        public async Task<List<TaxDTO>> GetAllTaxDTOs()
        {
            return await this.taxRepository.GetTaxDTOs();
        }

        public async Task<TaxGroupDTO> GetTaxGroupAsync()
        {
            return await this.taxRepository.GetTaxGroupAsync();
        }
        public async Task<bool> UpdateStatusAsync(int id, bool newStatus)
        {
            var oldTax = await this.taxRepository.GetActiveTax();
            if (oldTax != null&&oldTax.TaxId!=id&&newStatus)
            {
                oldTax.IsActive = false;
                await taxRepository.UpdateAsync(oldTax);
            }

            var tax = await this.taxRepository.GetByIdAsync(id);
            if (tax == null) return false;

            tax.IsActive = newStatus;
            await this.taxRepository.UpdateAsync(tax);
            return true;
        }

        public async Task<bool> DeleteUpcomingAsync(int id)
        {
            var tax = await this.taxRepository.GetByIdAsync(id);
            if (tax == null) return false;

            // chỉ cho phép xóa nếu là upcoming
            DateOnly now = DateOnly.FromDateTime(DateTime.Now);
            if (tax.EffectiveFrom > now)
            {
                await this.taxRepository.DeleteAsync(tax);
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateTaxAsync(int id, TaxEditDTO dto)
        {
            var entity = await this.taxRepository.GetByIdAsync(id);

            if (entity == null)
            {
                return false;
            }

            entity.TaxName = dto.TaxName;
            entity.EffectiveFrom = dto.EffectiveFrom;
            entity.EffectiveTo = dto.EffectiveTo;
            entity.IsActive = dto.IsActive;

            await taxRepository.RemoveBracketsAsync(id);

            if (dto.TaxBrackets != null)
            {
                foreach (var b in dto.TaxBrackets)
                {
                    entity.TaxBrackets.Add(new TaxBracket
                    {
                        LowerBound = b.LowerBound,
                        UpperBound = b.UpperBound,
                        Rate = b.Rate
                    });
                }
            }

            await taxRepository.UpdateAsync(entity);
            return true;
        }
    }
}
