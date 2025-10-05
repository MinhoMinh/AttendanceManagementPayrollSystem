using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface PayrollService
    {
        Task<PayrollRunDTO>  GeneratePayrollAsync(string name, int periodMonth, int periodYear, int createdBy);
    }
}
