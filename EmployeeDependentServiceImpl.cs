using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public class EmployeeDependentServiceImpl : EmployeeDependentService
    {
        private readonly EmployeeDependentRepository _dependentRepo;
        private readonly EmployeeRepository _employeeRepo;

        public EmployeeDependentServiceImpl(
            EmployeeDependentRepository dependentRepo,
            EmployeeRepository employeeRepo)
        {
            _dependentRepo = dependentRepo;
            _employeeRepo = employeeRepo;
        }

        public async Task<IEnumerable<EmployeeDependentDTO>> GetAllAsync()
        {
            var dependents = await _dependentRepo.GetAllAsync();
            var dtoList = new List<EmployeeDependentDTO>();

            foreach (var dependent in dependents)
            {
                var dto = await MapToDTO(dependent);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        public async Task<IEnumerable<EmployeeDependentDTO>> GetByEmployeeIdAsync(int employeeId)
        {
            var dependents = await _dependentRepo.GetByEmployeeIdAsync(employeeId);
            var dtoList = new List<EmployeeDependentDTO>();

            foreach (var dependent in dependents)
            {
                var dto = await MapToDTO(dependent);
                dtoList.Add(dto);
            }

            return dtoList;
        }

        public async Task<EmployeeDependentDTO?> GetByIdAsync(int id)
        {
            var dependent = await _dependentRepo.GetByIdAsync(id);
            if (dependent == null) return null;

            return await MapToDTO(dependent);
        }

        public async Task<EmployeeDependentDTO> CreateAsync(CreateEmployeeDependentDTO dto, int createdBy)
        {
            var dependent = new EmployeeDependent
            {
                EmployeeId = dto.EmployeeId,
                FullName = dto.FullName,
                Relationship = dto.Relationship,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                NationalId = dto.NationalId,
                IsTaxDependent = dto.IsTaxDependent,
                EffectiveStartDate = dto.EffectiveStartDate,
                EffectiveEndDate = dto.EffectiveEndDate,
                Proof = dto.Proof
            };

            var created = await _dependentRepo.AddAsync(dependent, createdBy);
            return await MapToDTO(created);
        }

        public async Task<EmployeeDependentDTO?> UpdateAsync(UpdateEmployeeDependentDTO dto)
        {
            var dependent = new EmployeeDependent
            {
                DependentId = dto.DependentId,
                FullName = dto.FullName,
                Relationship = dto.Relationship,
                DateOfBirth = dto.DateOfBirth,
                Gender = dto.Gender,
                NationalId = dto.NationalId,
                IsTaxDependent = dto.IsTaxDependent,
                EffectiveStartDate = dto.EffectiveStartDate,
                EffectiveEndDate = dto.EffectiveEndDate,
                Proof = dto.Proof
            };

            var updated = await _dependentRepo.UpdateAsync(dependent);
            if (updated == null) return null;

            return await MapToDTO(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _dependentRepo.DeleteAsync(id);
        }

        private async Task<EmployeeDependentDTO> MapToDTO(EmployeeDependent dependent)
        {
            string createdByName = null;
            if (dependent.CreatedBy.HasValue)
            {
                var creator = await _employeeRepo.GetByIdAsync(dependent.CreatedBy.Value);
                createdByName = creator?.EmpName;
            }

            return new EmployeeDependentDTO
            {
                DependentId = dependent.DependentId,
                EmployeeId = dependent.EmployeeId,
                EmployeeName = dependent.Employee?.EmpName,
                FullName = dependent.FullName,
                Relationship = dependent.Relationship,
                DateOfBirth = dependent.DateOfBirth,
                Gender = dependent.Gender,
                NationalId = dependent.NationalId,
                IsTaxDependent = dependent.IsTaxDependent,
                EffectiveStartDate = dependent.EffectiveStartDate,
                EffectiveEndDate = dependent.EffectiveEndDate,
                CreatedBy = dependent.CreatedBy,
                CreatedByName = createdByName,
                CreatedDate = dependent.CreatedDate,
                Proof = dependent.Proof
            };
        }
    }
}
