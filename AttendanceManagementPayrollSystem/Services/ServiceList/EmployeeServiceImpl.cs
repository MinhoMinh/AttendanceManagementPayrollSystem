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
            string pto = "12.0|0.0|12.0";
            string sick = "12.0|0.0|12.0";
            string vacation = "12.0|0.0|12.0";
            string overtime = "0.0|0.0|0.0";

            var employee = new Employee
            {
                EmpName = dto.EmpName,
                EmpDob = dto.EmpDob,
                EmpPhoneNumber = dto.EmpPhoneNumber,
                DepId = dto.DepId,
                Username = dto.Username,
                PasswordHash = dto.Password,
                IsActive = true
            };

            await employeeRepository.AddAsync(employee);

            var balance = new EmployeeBalance
            {
                EmpId = employee.EmpId,  // sau khi SaveChanges(), EF tự gán ID
                Pto = pto,
                Sick = sick,
                Vacation = vacation,
                Overtime = overtime,
                LastUpdated = DateTime.Now
            };

            await this.employeeRepository.AddBalenceForNewEmployee(balance);
        }


        public async Task<List<EmployeeBasicDTO>> GetAllEmployeeBasic()
        {
            return await this.employeeRepository.GetAllEmployeeBasic();
        }

        public async Task<IEnumerable<EmployeeViewDTO>> GetAllEmployeesAsync()
        {
            var employees = await this.employeeRepository.GetAllEmployeesAsync(); // vẫn trả chuỗi

            return employees.Select(e => new EmployeeViewDTO
            {
                EmpId = e.EmpId,
                EmpName = e.EmpName,
                EmpDob = e.EmpDob,
                EmpPhoneNumber = e.EmpPhoneNumber,
                DepId = e.DepId,
                DepName = e.Dep?.DepName ?? string.Empty,
                Username = e.Username,
                Pto = ParseBalance(e.EmployeeBalance?.Pto),
                Sick = ParseBalance(e.EmployeeBalance?.Sick),
                Vacation = ParseBalance(e.EmployeeBalance?.Vacation),
                Overtime = ParseBalance(e.EmployeeBalance?.Overtime),
                LastUpdated = e.EmployeeBalance?.LastUpdated
            });
        }

        public async Task<EmployeeViewDTO> GetEmployeeViewByIdAsync(int id)
        {
            var employee = await this.employeeRepository.GetByIdAsync(id); // vẫn trả chuỗi

            return new EmployeeViewDTO
            {
                EmpId = employee.EmpId,
                EmpName = employee.EmpName,
                EmpDob = employee.EmpDob,
                EmpPhoneNumber = employee.EmpPhoneNumber,
                DepId = employee.DepId,
                DepName = employee.Dep?.DepName ?? string.Empty,
                Username = employee.Username,
                Pto = ParseBalance(employee.EmployeeBalance?.Pto),
                Sick = ParseBalance(employee.EmployeeBalance?.Sick),
                Vacation = ParseBalance(employee.EmployeeBalance?.Vacation),
                Overtime = ParseBalance(employee.EmployeeBalance?.Overtime),
                LastUpdated = employee.EmployeeBalance?.LastUpdated
            };
        }

        public async Task<IEnumerable<EmployeeGroupByDepartmentDTO>> GetEmployeesGroupedByDepartmentAsync()
        {
            return await this.employeeRepository.GetEmployeesGroupedByDepartmentAsync();
        }

        private BalanceDTO ParseBalance(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                Console.WriteLine("ParseBalance lỗi này");
                return new BalanceDTO { Remaining = 0, Used = 0, Total = 0 };
            }
                

            var parts = str.Split('|').Select(double.Parse).ToArray();
            return new BalanceDTO
            {
                Remaining = parts[0],
                Used = parts[1],
                Total = parts[2]
            };
        }

        public async Task<bool> UpdateStatusAsync(EmployeeStatusUpdateDTO dto)
        {
            var emp = await this.employeeRepository.GetByIdAsync(dto.EmpId);
            if (emp == null) return false;

            emp.IsActive = dto.IsActive;

            await this.employeeRepository.UpdateAsync(emp);
            return true;
        }
    }
}
