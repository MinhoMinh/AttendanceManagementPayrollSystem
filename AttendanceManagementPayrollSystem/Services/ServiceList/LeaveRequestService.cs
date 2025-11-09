using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.DTOs;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface LeaveRequestService
    {
        Task<LeaveRequestDTO> AddAsync(LeaveRequestDTO dto);
        Task<IEnumerable<LeaveRequestDTO>> GetByEmployeeIdAsync(int empId);
    }
}

