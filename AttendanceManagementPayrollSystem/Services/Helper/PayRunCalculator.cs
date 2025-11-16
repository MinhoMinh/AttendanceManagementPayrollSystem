using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.Helper
{
    public class PayRunCalculator
    {
        public static PayRunItemDto CalculatePay(PayRunContext context, Employee employee,
            WeeklyShiftDto shift, List<OvertimeRequest>? overtimes, List<Bonu> bonus,
            List<LeaveRequest> leaves)
        {

            PayRunItemDto itemDto = new PayRunItemDto
            {
                EmpId = employee.EmpId,
                EmpName = employee.EmpName,
                Notes = ""
            };

            //earning
            //-------------
            //

            //clockin
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

            var empHolidays = context.Holidays
                    .SelectMany(h => h.DepartmentHolidays)
                    .Where(dhc => dhc.DepId == employee.DepId)
                    .ToList();

            foreach (var holiday in empHolidays)
            {
                holidayUnits = 0m;

                // Restrict holiday within the pay period
                var start = holiday.StartDate.Date < context.PeriodStart.Date ? context.PeriodStart.Date : holiday.StartDate.Date;
                var end = holiday.EndDate.Date > context.PeriodEnd.Date ? context.PeriodEnd.Date : holiday.EndDate.Date;

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
                    decimal holidayValue = holidayUnits * context.SalaryPolicy.WorkUnitValue; // same rate as you use for work units
                    var holidayComponent = new PayRunComponentDto
                    {
                        ComponentType = "Earning",
                        ComponentCode = "BONUS",
                        Description = $"Lương nghỉ lễ {holiday.HolidayName}: {holidayUnits} công",
                        Amount = holidayValue,
                        Taxable = true,
                        Insurable = true
                    };
                    itemDto.Components.Add(holidayComponent);
                    itemDto.GrossPay += holidayValue;
                }
            }

            //overtimes
            if (overtimes != null && overtimes.Count > 0)
            {
                var sumWorkhour = overtimes.Sum(o => o.Hours ?? 0m);
                if (sumWorkhour > 0)
                {
                    decimal value = (sumWorkhour / 4m) * context.SalaryPolicy.WorkUnitValue;
                    var componentDto = new PayRunComponentDto
                    {
                        ComponentType = "Earning",
                        ComponentCode = "OVERTIME",
                        Description = $"Làm thêm: {sumWorkhour} giờ",
                        Amount = value,
                        Taxable = true,
                        Insurable = false
                    };

                    itemDto.Components.Add(componentDto);
                    itemDto.GrossPay += value;
                }
            }


            //bonus
            if (bonus != null && bonus.Count > 0)
            {
                foreach (var bonu in bonus)
                {
                    decimal value = bonu.BonusAmount;
                    var componentDto = new PayRunComponentDto
                    {
                        ComponentType = "Earning",
                        ComponentCode = "BONUS",
                        Description = $"Thưởng: {bonu.BonusName}",
                        Amount = value,
                        Taxable = true,
                        Insurable = true
                    };

                    itemDto.Components.Add(componentDto);
                    itemDto.GrossPay += value;
                }
            }

            //deduction
            //-------------
            //

            decimal insuranable = itemDto.Components.Sum(c => c.Insurable && c.ComponentType == "Earning" ?
                c.Amount : 0);
            decimal totalinsurance = 0m;

            if (insuranable > 0)
            {
                var value = insuranable * context.SocialInsurance.EmployeeRate;
                itemDto.Components.Add(new PayRunComponentDto
                {
                    ComponentType = "Deduction",
                    ComponentCode = "INSURANCE",
                    Description = $"Bảo hiểm xã hội: {context.SocialInsurance.EmployeeRate}",
                    Amount = value,
                    Taxable = false,
                    Insurable = false
                });
                itemDto.Deductions += value;
                totalinsurance += value;


                value = insuranable * context.HealthInsurance.EmployeeRate;
                itemDto.Components.Add(new PayRunComponentDto
                {
                    ComponentType = "Deduction",
                    ComponentCode = "INSURANCE",
                    Description = $"Bảo hiểm sức khoẻ: {context.HealthInsurance.EmployeeRate}",
                    Amount = value,
                    Taxable = false,
                    Insurable = false
                });
                itemDto.Deductions += value;
                totalinsurance += value;


                value = insuranable * context.UnemployeeInsurance.EmployeeRate;
                itemDto.Components.Add(new PayRunComponentDto
                {
                    ComponentType = "Deduction",
                    ComponentCode = "INSURANCE",
                    Description = $"Bảo hiểm thất nghiệp: {context.UnemployeeInsurance.EmployeeRate}",
                    Amount = value,
                    Taxable = false,
                    Insurable = false
                });
                itemDto.Deductions += value;
                totalinsurance += value;
            }
                
            decimal taxable = itemDto.Components.Sum(c => c.Taxable && c.ComponentType == "Earning" ?
                c.Amount : 0) - totalinsurance;
            if (taxable > 0)
            {
                var value = CalculateTax(taxable, context.Tax.TaxBrackets.ToList());
                itemDto.Components.Add(new PayRunComponentDto
                {
                    ComponentType = "Deduction",
                    ComponentCode = "TAX",
                    Description = $"Thuế thu nhập cá nhân",
                    Amount = value,
                    Taxable = false,
                    Insurable = false
                });
                itemDto.Deductions += value;
            }

            itemDto.NetPay = itemDto.GrossPay - itemDto.Deductions;
            return itemDto;
        }

        public static decimal CalculateTax(decimal taxable, List<TaxBracket> brackets)
        {
            decimal totalTax = 0;

            var ordered = brackets
                .OrderBy(b => b.LowerBound)
                .ToList();

            foreach (var b in ordered)
            {
                decimal upper = b.UpperBound ?? decimal.MaxValue;

                if (taxable <= b.LowerBound)
                    continue;

                decimal taxableInBracket = Math.Min(taxable, upper) - b.LowerBound;

                if (taxableInBracket > 0)
                {
                    totalTax += taxableInBracket * b.Rate;
                }

                if (taxable <= upper)
                    break;
            }

            return totalTax;
        }
    }
}
