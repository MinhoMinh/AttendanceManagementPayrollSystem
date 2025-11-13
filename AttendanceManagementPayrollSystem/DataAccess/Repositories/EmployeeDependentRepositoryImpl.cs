using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class EmployeeDependentRepositoryImpl : BaseRepositoryImpl, EmployeeDependentRepository
    {
        public EmployeeDependentRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        public async Task<EmployeeDependentDTO> AddDependentAsync(EmployeeDependentCreateDTO dto)
        {
            var entity = new EmployeeDependent
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
                CreatedBy = dto.CreatedBy,
                CreatedDate = DateTime.Now,
                Proof = dto.Proof
            };

            this._context.Add(entity);
            await this._context.SaveChangesAsync();

            return new EmployeeDependentDTO
            {
                DependentId = entity.DependentId,
                FullName = entity.FullName,
                Relationship = entity.Relationship,
                DateOfBirth = entity.DateOfBirth,
                Gender = entity.Gender,
                NationalId = entity.NationalId,
                IsTaxDependent = entity.IsTaxDependent,
                EffectiveStartDate = entity.EffectiveStartDate,
                EffectiveEndDate = entity.EffectiveEndDate,
                CreatedDate = entity.CreatedDate,
                Proof = entity.Proof
            };
        }

        public async Task<List<EmployeeWithDependentsDTO>> GetDependentsGroupedByEmployeeAsync()
        {
            return await this._context.EmployeeDependents
                .Include(ed => ed.Employee)
                .GroupBy(e => new { e.EmployeeId, e.Employee.EmpName })
                .Select(g => new EmployeeWithDependentsDTO
                {
                    EmployeeId = g.Key.EmployeeId,
                    EmployeeName = g.Key.EmpName,
                    Dependents = g.Select(dep => new EmployeeDependentDTO
                    {
                        DependentId = dep.DependentId,
                        FullName = dep.FullName,
                        Relationship = dep.Relationship,
                        DateOfBirth = dep.DateOfBirth,
                        Gender = dep.Gender,
                        NationalId = dep.NationalId,
                        IsTaxDependent = dep.IsTaxDependent,
                        EffectiveStartDate = dep.EffectiveStartDate,
                        EffectiveEndDate = dep.EffectiveEndDate,
                        CreatedDate = dep.CreatedDate,
                        Proof = dep.Proof
                    }).ToList()
                })
                .ToListAsync();
        }
    }
}
