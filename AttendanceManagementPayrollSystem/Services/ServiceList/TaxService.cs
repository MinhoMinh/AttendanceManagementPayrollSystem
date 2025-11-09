using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface TaxService
    {
        Task<List<TaxDTO>> GetAllTaxDTOs();
        Task<TaxDTO?> GetActiveTaxDTO();
        Task<bool> AddTaxAsync(TaxEditDTO dto);
        Task<TaxGroupDTO> GetTaxGroupAsync();
        Task<bool> UpdateStatusAsync(int id, bool newStatus);
        Task<bool> DeleteUpcomingAsync(int id);

        Task<bool> UpdateTaxAsync(int id, TaxEditDTO dto);

    }
}
