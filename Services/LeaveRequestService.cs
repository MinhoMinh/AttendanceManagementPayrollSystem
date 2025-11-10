using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface LeaveRequestService
    {
        Task<LeaveRequestDTO> AddAsync(LeaveRequestDTO dto);
        Task<IEnumerable<LeaveRequestDTO>> GetByEmployeeIdAsync(int empId);
        Task<IEnumerable<LeaveRequestDTO>> GetPendingAsync();
        Task UpdateStatusAsync(LeaveRequestStatusDTO dto);
    }
}

