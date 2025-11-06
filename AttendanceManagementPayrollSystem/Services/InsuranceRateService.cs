using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface InsuranceRateService
    {
        Task<List<InsuranceRateDTO>> GetInsuranceRateSetDTOs();

        Task<List<InsuranceRateDTO>> GetActiveInsuranceRateSetDTO();

        Task<List<int>> GetActiveIds();
        Task<bool> UpdateUpcomingAsync(InsuranceRateDTO dto);
        Task<bool> UpdateActiveAsync(InsuranceRateDTO dto);
        Task<Dictionary<string, List<int>>> GetUpcomingInsuranceRateIds();
        Task<bool> AddInsuranceRate(InsuranceRateDTO dto);
        Task<List<InsuranceRateGroupDTO>> GetInsuranceRateGroupsAsync();

        Task RemoveUpcomingInsuranceByIdAsync(int id);

        Task UpdateStatusAsync(int id, bool isActive);
    }
}
