using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.Helper
{
    public class PayRunCalculator
    {
        public static PayRunItemDto CalculatePay(Employee employee)
        {
            PayRunItemDto itemDto = new PayRunItemDto
            {
                EmpId = employee.EmpId,
                EmpName = employee.EmpName,
                Notes = ""
            };

            var clockin = employee.Clockins.FirstOrDefault();
            if (clockin != null)
            {
                decimal actualClockinValue = (clockin.WorkUnits ??= 0) * 200000m;
                decimal expectedClockinValue = (clockin.ScheduledUnits ??= 0) * 200000m;
                if (actualClockinValue > 0)
                {
                    var componentDto = new PayRunComponentDto
                    {
                        ComponentType = "Earning",
                        ComponentCode = "BASIC",
                        Description = $"Clockin: {clockin.WorkUnits} workhour",
                        Amount = actualClockinValue,
                        Taxable = true,
                        Insurable = true
                    };

                    itemDto.Components.Add(componentDto);
                    itemDto.GrossPay += actualClockinValue;
                }

                var kpi = employee.KpiEmps.FirstOrDefault();
                if (kpi != null)
                {
                    decimal score = 0m;

                    foreach (var kpiCom in kpi.Kpicomponents)
                    {
                        // score is on 0 - 10 scale. weight is on 0 - 100 scale
                        decimal componentScore = (kpiCom.AssignedScore ?? kpiCom.SelfScore ?? 0) * kpiCom.Weight * 0.001m;
                        score += componentScore;
                    }
                    decimal kpiValue = score * ((kpi.Prorate != null && kpi.Prorate == true) ? actualClockinValue : expectedClockinValue);

                    if (kpiValue > 0)
                    {
                        var componentDto = new PayRunComponentDto
                        {
                            ComponentType = "Earning",
                            ComponentCode = "BONUS",
                            Description = $"Kpi: {(score * 10m):F2}/10 score",
                            Amount = kpiValue,
                            Taxable = true,
                            Insurable = true
                        };

                        itemDto.Components.Add(componentDto);
                        itemDto.GrossPay += actualClockinValue;
                    }
                }
            }

            return itemDto;
        }
    }
}
