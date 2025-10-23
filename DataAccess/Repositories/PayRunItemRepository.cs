using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface PayRunItemRepository : BaseRepository
    {
        Task<List<PayRunItemDTO>> GetPayRunItemsByEmpIdAsync(int empId);


    }
}
