using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public class DepartmentServiceImpl : DepartmentService
    {
        private readonly DepartmentRepository _departmentRepo;

        public DepartmentServiceImpl(DepartmentRepository departmentRepo)
        {
            _departmentRepo = departmentRepo;
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllAsync()
        {
            var departments = await _departmentRepo.GetAllAsync();
            return departments.Select(d => new DepartmentDTO
            {
                DepId = d.DepId,
                DepName = d.DepName
            });
        }

        public async Task<IEnumerable<EmployeeBasicDTO>> GetEmployeesAsync(int headId)
        {
            var employees = await _departmentRepo.GetEmployees(headId);

            if (employees == null)
                return Enumerable.Empty<EmployeeBasicDTO>();

            return employees.Select(e => new EmployeeBasicDTO
            {
                EmpId = e.EmpId,
                EmpName = e.EmpName
                // map others
            });
        }

        public async Task<DepartmentDTO?> GetByIdAsync(int id)
        {
            var dep = await _departmentRepo.GetByIdAsync(id);
            if (dep == null) return null;

            return new DepartmentDTO
            {
                DepId = dep.DepId,
                DepName = dep.DepName
            };
        }

        public async Task<IEnumerable<DepartmentDTO>> GetAllDepartmentExceptManager()
        {
            var departments = await _departmentRepo.GetAllDepartmentExceptManager();
            return departments.Select(d => new DepartmentDTO
            {
                DepId = d.DepId,
                DepName = d.DepName
            });
        }
    }
}
