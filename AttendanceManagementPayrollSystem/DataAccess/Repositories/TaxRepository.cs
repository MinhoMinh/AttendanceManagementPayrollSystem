using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface TaxRepository : BaseRepository
    {
        Task<List<TaxDTO>> GetTaxDTOs();
        Task<TaxDTO?> GetActiveTaxDTO();
        Task<Tax?> GetActiveTax();
        Task<bool> AddTaxAsync(TaxEditDTO dto);
        Task<TaxGroupDTO> GetTaxGroupAsync();
        Task<Tax?> GetByIdAsync(int id);
        Task UpdateAsync(Tax tax);
        Task DeleteAsync(Tax tax);
        Task RemoveBracketsAsync(int id);
    }
}
