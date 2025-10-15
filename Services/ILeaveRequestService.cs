using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.DTOs;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface ILeaveRequestService
    {
        Task<LeaveRequestDTO> AddAsync(LeaveRequestDTO dto);
        Task<IEnumerable<LeaveRequestDTO>> GetByEmployeeIdAsync(int empId);
    }
}

