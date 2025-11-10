using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface ClockinComponentService
    {
        Task<bool> UpdateByRespond(ClockinComponentRespondDTO dto);
    }
}
