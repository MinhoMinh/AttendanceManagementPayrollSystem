using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface EmployeeService
    {
        Task CreateAsync(EmployeeCreateDTO dto);

        Task<List<EmployeeBasicDTO>> GetAllEmployeeBasic();

        Task<IEnumerable<EmployeeViewDTO>> GetAllEmployeesAsync();

        Task<EmployeeViewDTO> GetEmployeeViewByIdAsync(int id);


        Task<IEnumerable<EmployeeGroupByDepartmentDTO>> GetEmployeesGroupedByDepartmentAsync();

        Task<bool> UpdateStatusAsync(EmployeeStatusUpdateDTO dto);
    }
}
