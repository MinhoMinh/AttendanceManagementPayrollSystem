using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ILeaveRequestRepository : BaseRepository
    {
        Task<LeaveRequest> AddAsync(LeaveRequest request);
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int empId);

        Task<IEnumerable<LeaveRequest>> GetLeaveByEmployeeId(int empId, DateOnly? startDate, DateOnly? endDate);

        IEnumerable<LeaveType> GetRates();
    }
}


