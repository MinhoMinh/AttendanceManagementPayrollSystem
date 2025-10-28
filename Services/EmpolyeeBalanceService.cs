using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface IEmployeeBalanceService
    {
        Task<EmployeeBalanceDto?> GetBalanceAsync(int empId);
    }
}
