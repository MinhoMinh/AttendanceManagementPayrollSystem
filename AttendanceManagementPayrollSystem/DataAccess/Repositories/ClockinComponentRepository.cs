using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ClockinComponentRepository:BaseRepository
    {
        Task UpdateAsync(ClockinComponent clockinComponent);
        Task<ClockinComponent> GetById(int id);
    }
}
