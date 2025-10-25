using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using System.Text.RegularExpressions;

namespace AttendanceManagementPayrollSystem.Services.Mapper
{
    public class EmployeeBalanceMapper
    {
        private static readonly string pattern = @"^\d+(\.\d+)?\|\d+(\.\d+)?\|\d+(\.\d+)?$";

        public static EmployeeBalanceDto? ToDto(EmployeeBalance balance)
        {
            if (balance == null) return null;

            var dto = new EmployeeBalanceDto
            {
                Id = balance.Id,
                EmpId = balance.EmpId,
                LastUpdated = balance.LastUpdated
            };

            if (Regex.IsMatch(balance.Pto, pattern))
            {
                var parts = balance.Pto.Split('|')
                                 .Select(x => decimal.Parse(x))
                                 .ToArray();
                dto.PtoAvailable = parts[0];
                dto.PtoUsed = parts[1];
                dto.PtoTotal = parts[2];
            }

            if (Regex.IsMatch(balance.Sick, pattern))
            {
                var parts = balance.Sick.Split('|')
                                 .Select(x => decimal.Parse(x))
                                 .ToArray();
                dto.SickAvailable = parts[0];
                dto.SickUsed = parts[1];
                dto.SickTotal = parts[2];
            }

            if (Regex.IsMatch(balance.Vacation, pattern))
            {
                var parts = balance.Vacation.Split('|')
                                 .Select(x => decimal.Parse(x))
                                 .ToArray();
                dto.VacationAvailable = parts[0];
                dto.VacationUsed = parts[1];
                dto.VacationTotal = parts[2];
            }

            if (Regex.IsMatch(balance.Overtime, pattern))
            {
                var parts = balance.Overtime.Split('|')
                                 .Select(x => decimal.Parse(x))
                                 .ToArray();
                dto.OvertimeAvailable = parts[0];
                dto.OvertimeUsed = parts[1];
                dto.OvertimeTotal = parts[2];
            }

            return dto;
        }
    
        public static EmployeeBalance? ToEntity(EmployeeBalanceDto dto)
        {
            if (dto == null) return null;

            var balance = new EmployeeBalance
            {
                Id = dto.Id,
                EmpId = dto.EmpId,
                LastUpdated = dto.LastUpdated,
                Pto = $"{dto.PtoAvailable:F1}|{dto.PtoUsed:F1}|{dto.PtoTotal:F1}",
                Sick = $"{dto.SickAvailable:F1}|{dto.SickUsed:F1}|{dto.SickTotal:F1}",
                Vacation = $"{dto.VacationAvailable:F1}|{dto.VacationUsed:F1}|{dto.VacationTotal:F1}",
                Overtime = $"{dto.OvertimeAvailable:F1}|{dto.OvertimeUsed:F1}|{dto.OvertimeTotal:F1}",
            };

            return balance;
        }
    }
}
