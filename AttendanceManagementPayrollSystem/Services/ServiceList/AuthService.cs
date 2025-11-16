using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.Models;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface AuthService
    {
        Task<EmployeeDTO?> LoginAsync(string username, string password);
    }

    public class AuthServiceImpl : AuthService
    {
        private readonly EmployeeRepository _employeeRepo;

        public AuthServiceImpl(EmployeeRepository employeeRepo)
        {
            _employeeRepo = employeeRepo;
        }

        public async Task<EmployeeDTO?> LoginAsync(string username, string password)
        {
            var employee = await _employeeRepo.FindByUsernameAsync(username);
            if (employee == null)
                return null;

            if (!VerifyPassword(password, employee.PasswordHash))
                return null;

            await _employeeRepo.LoadRoles(employee);

            var activeRole = employee.EmployeeRoles.FirstOrDefault(r => r.IsActive);
            if (activeRole?.Role == null)
                return null;

            var empRole = activeRole.Role;

            return new EmployeeDTO
            {
                EmpId = employee.EmpId,
                EmpName = employee.EmpName,
                EmpRole = empRole.RoleName,
                Permissions = empRole.Permissions.Select(r => r.PermissionName).ToList()
            };
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Nếu sau này bạn hash password bằng SHA256, chỉ cần mở lại đoạn này
            using (SHA256 sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(bytes).Replace("-", "").ToLower();
                return hash == storedHash;
            }

            // Nếu bạn chưa hash, tạm dùng:
            //return password == storedHash;
        }
    }
}
