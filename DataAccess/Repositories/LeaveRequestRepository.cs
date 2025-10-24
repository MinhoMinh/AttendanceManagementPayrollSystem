    using System.Threading.Tasks;
    using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ILeaveRequestRepository : BaseRepository
    {
        public interface ILeaveRequestRepository
        {
            Task<LeaveRequest> AddAsync(LeaveRequest request);
            Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int empId);
        }
    }


