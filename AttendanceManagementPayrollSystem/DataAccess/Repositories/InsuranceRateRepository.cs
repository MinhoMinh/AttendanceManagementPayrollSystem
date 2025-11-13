using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface InsuranceRateRepository:BaseRepository
    {
        Task<List<InsuranceRateDTO>> GetInsuranceRateDTOs();

        Task<List<InsuranceRateDTO>> GetActiveInsuranceRateDTO();

        Task<List<int>> GetActiveIds();
        Task<InsuranceRate?> GetById(int id);

        Task<List<InsuranceRate>> GetActiveInsurancesInTime(DateTime start, DateTime end);
        Task UpdateAsync(InsuranceRate insuranceRate);
        Task AddAsync(InsuranceRate entity);
        Task<Dictionary<string,List<int>>> GetUpcomingInsuranceRateIds();
        Task<List<InsuranceRateGroupDTO>> GetInsuranceRateGroupsAsync();
        Task RemoveAsync(InsuranceRate insuranceRate);

        Task<List<InsuranceRate>> GetByCategoryAndActiveAsync(string category);

    }
}
