using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.Helper
{
    public class PayRunCalculator
    {
        public static PayRunItemDto CalculatePay(Employee employee, 
            WeeklyShiftDto shift, List<DepartmentHolidayCalendarDTO> holidays,
            DateTime periodStart, DateTime periodEnd)
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
                        Description = $"Chấm công: {clockin.WorkUnits} công",
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

            //holiday
            decimal holidayUnits = 0m;

            foreach (var holiday in holidays)
            {
                holidayUnits = 0m;

                // Restrict holiday within the pay period
                var start = holiday.StartDate.Date < periodStart.Date ? periodStart.Date : holiday.StartDate.Date;
                var end = holiday.EndDate.Date > periodEnd.Date ? periodEnd.Date : holiday.EndDate.Date;

                for (var date = start; date <= end; date = date.AddDays(1))
                {
                    var dailyShift = shift.GetDailyShift(date.DayOfWeek);
                    if (dailyShift != null)
                    {
                        holidayUnits += dailyShift.ShiftDtos.Sum(s => s.WorkUnits);
                    }
                }

                if (holidayUnits > 0)
                {
                    decimal holidayValue = holidayUnits * 200000m; // same rate as you use for work units
                    var holidayComponent = new PayRunComponentDto
                    {
                        ComponentType = "Earning",
                        ComponentCode = "HOLIDAY",
                        Description = $"Lương nghỉ lễ {holiday.HolidayName}: {holidayUnits} công",
                        Amount = holidayValue,
                        Taxable = true,
                        Insurable = true
                    };
                    itemDto.Components.Add(holidayComponent);
                    itemDto.GrossPay += holidayValue;
                }
            }

            return itemDto;
        }
    }
}
