using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using System.Globalization;

namespace AttendanceManagementPayrollSystem.Services.Impl
{
    public class EmployeeBalanceServiceImpl : IEmployeeBalanceService
    {
        private readonly IEmployeeBalanceRepository _repo;

        public EmployeeBalanceServiceImpl(IEmployeeBalanceRepository repo)
        {
            _repo = repo;
        }

        public async Task<EmployeeBalanceDto?> GetBalanceAsync(int empId)
        {
            var entity = await _repo.GetByEmployeeIdAsync(empId);
            if (entity == null) return null;

            var dto = new EmployeeBalanceDto
            {
                Id = entity.Id,
                EmpId = entity.EmpId,
                LastUpdated = entity.LastUpdated
            };

            (dto.PtoAvailable, dto.PtoUsed, dto.PtoTotal) = ParseTriple(entity.Pto);
            (dto.SickAvailable, dto.SickUsed, dto.SickTotal) = ParseTriple(entity.Sick);
            (dto.VacationAvailable, dto.VacationUsed, dto.VacationTotal) = ParseTriple(entity.Vacation);
            (dto.OvertimeAvailable, dto.OvertimeUsed, dto.OvertimeTotal) = ParseTriple(entity.Overtime);

            return dto;
        }

        private static (decimal available, decimal used, decimal total) ParseTriple(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return (0, 0, 0);

            var parts = input.Split('|');
            if (parts.Length != 3) return (0, 0, 0);

            decimal.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out var a);
            decimal.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out var b);
            decimal.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out var c);

            return (a, b, c);
        }
    }
}
