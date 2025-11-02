using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface TaxService
    {
        Task<List<TaxDTO>> GetAllTaxDTOs();
        Task<TaxDTO?> GetActiveTaxDTOs();
        Task<TaxDTO> AddTaxAsync(TaxEditDTO dto);
    }
}
