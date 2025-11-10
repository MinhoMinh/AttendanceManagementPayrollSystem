using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface ClockinComponentService
    {
        Task<bool> UpdateByRespond(ClockinComponentRespondDTO dto);
    }
}
