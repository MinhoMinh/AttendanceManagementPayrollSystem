using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.DTOs;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface LeaveRequestService
    {
        Task<LeaveRequestDTO> AddAsync(LeaveRequestDTO dto);
        Task<IEnumerable<LeaveRequestDTO>> GetByEmployeeIdAsync(int empId);

        Task<IEnumerable<LeaveRequestDTO>> GetLeaveHistoryByEmployee(int empId, DateOnly? startDate, DateOnly? endDate);

        IEnumerable<LeaveType> GetRates();

        Task<IEnumerable<LeaveRequestDTO>> GetAllLeaveRequestByDate(DateOnly? startDate, DateOnly? endDate);

        Task<LeaveRequest> UpdateApprovalAsync(LeaveRequestApprovalDTO dto);

        Task<List<LeaveRequestGroupDTO>> GetLeaveRequestGroupByDepIdAndDateRange(int depId, DateTime from, DateTime to);
    }
}

