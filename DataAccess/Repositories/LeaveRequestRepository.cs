using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ILeaveRequestRepository : BaseRepository
    {
        Task<LeaveRequest> AddAsync(LeaveRequest request);
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int empId);
        Task<IEnumerable<LeaveRequest>> GetPendingAsync();
        Task<LeaveRequest?> GetByIdAsync(int id);
        Task UpdateAsync(LeaveRequest request);
    }
}


