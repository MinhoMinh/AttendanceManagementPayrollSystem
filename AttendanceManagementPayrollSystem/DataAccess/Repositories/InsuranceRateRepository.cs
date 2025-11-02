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
        Task UpdateAsync(InsuranceRate insuranceRate);
        Task AddAsync(InsuranceRate entity);
        
    }
}
