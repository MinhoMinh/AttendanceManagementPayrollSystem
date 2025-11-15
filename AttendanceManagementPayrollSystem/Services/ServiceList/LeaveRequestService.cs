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
    }
}

