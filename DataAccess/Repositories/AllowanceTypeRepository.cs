using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface AllowanceTypeRepository : BaseRepository
    {
        Task<IEnumerable<AllowanceType>> GetAllAsync();
        Task<AllowanceType?> GetByIdAsync(int id);
        Task<IEnumerable<AllowanceType>> SearchAsync(string searchTerm);
        Task<AllowanceType> AddAsync(AllowanceType allowanceType);
        Task<AllowanceType?> UpdateAsync(AllowanceType allowanceType);
        Task<bool> DeleteAsync(int id);
    }
}