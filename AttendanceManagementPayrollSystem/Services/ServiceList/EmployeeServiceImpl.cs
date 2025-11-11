using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public class EmployeeServiceImpl : EmployeeService
    {
        private readonly EmployeeRepository employeeRepository;

        public EmployeeServiceImpl(EmployeeRepository employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public async Task CreateAsync(EmployeeCreateDTO dto)
        {
            var entity = new Employee
            {
                EmpName = dto.EmpName,
                EmpDob = dto.EmpDob,
                EmpPhoneNumber = dto.EmpPhoneNumber,
                DepId = dto.DepId,
                Username = dto.Username,
                //PasswordHash = HashPassword(dto.Password)
                PasswordHash = dto.Password
            };

            await employeeRepository.AddAsync(entity);
        }
    }
}
