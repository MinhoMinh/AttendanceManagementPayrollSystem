using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface EmployeeService
    {
        Task CreateAsync(EmployeeCreateDTO dto);

    }
}
