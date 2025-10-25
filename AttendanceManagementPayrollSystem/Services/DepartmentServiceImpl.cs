using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
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
    }
}
