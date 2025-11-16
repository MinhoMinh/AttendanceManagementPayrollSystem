using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ILeaveRequestRepository : BaseRepository
    {
        Task<LeaveRequest> AddAsync(LeaveRequest request);
        Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int empId);

        Task<IEnumerable<LeaveRequest>> GetLeaveByEmployeeId(int empId, DateOnly? startDate, DateOnly? endDate);

        Task<IEnumerable<LeaveRequest>> GetApprovedLeaveByDate(DateOnly? startDate, DateOnly? endDate);

        IEnumerable<LeaveType> GetRates();

        Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestByDate(DateOnly? startDate, DateOnly? endDate);

        Task<LeaveRequest> UpdateApprovalAsync(LeaveRequest entity);

        Task<LeaveRequest> GetByIdAsync(int id);

        Task<List<LeaveRequestGroupDTO>> GetGroupByDepIdAndDateRange(int depId, DateTime from, DateTime to);
    }
}


