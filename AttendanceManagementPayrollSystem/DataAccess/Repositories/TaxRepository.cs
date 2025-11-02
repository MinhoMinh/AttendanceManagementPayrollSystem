using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface TaxRepository : BaseRepository
    {
        Task<List<TaxDTO>> GetTaxDTOs();
        Task<TaxDTO?> GetActiveTaxDTOs();
        Task<Tax> AddTaxAsync(TaxEditDTO dto);

    }
}
