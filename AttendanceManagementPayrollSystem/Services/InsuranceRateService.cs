using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface InsuranceRateService
    {
        Task<List<InsuranceRateDTO>> GetInsuranceRateSetDTOs();

        Task<List<InsuranceRateDTO>> GetActiveInsuranceRateSetDTO();

        Task<List<int>> GetActiveIds();
        Task<bool> UpdateInactiveAsync(InsuranceRateDTO dto);
        Task<bool> UpdateActiveAsync(InsuranceRateDTO dto);
    }
}
