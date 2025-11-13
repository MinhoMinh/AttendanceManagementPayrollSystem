using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public class EmployeeDependentServiceImpl:EmployeeDependentService
    {
        private readonly EmployeeDependentRepository _repo;

        public EmployeeDependentServiceImpl(EmployeeDependentRepository repo)
        {
            _repo = repo;
        }

        public async Task<EmployeeDependentDTO> AddDependentAsync(EmployeeDependentCreateDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FullName))
                throw new ArgumentException("Full name is required.");

            if (string.IsNullOrWhiteSpace(dto.Relationship))
                throw new ArgumentException("Relationship is required.");

            return await _repo.AddDependentAsync(dto);
        }

        public async Task<List<EmployeeWithDependentsDTO>> GetDependentsGroupedByEmployeeAsync()
        {
            return await _repo.GetDependentsGroupedByEmployeeAsync();
        }
    }
}
