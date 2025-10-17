using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AttendanceManagementPayrollSystem.Services
{
    public class PayRunServiceImpl : PayRunService
    {
        private readonly PayRunRepository _payRunRepo;
        private readonly ClockinRepository _clockinRepo;
        private readonly EmployeeRepository _employeeRepo;  


        public PayRunServiceImpl(ClockinRepository clockinRepo, PayRunRepository payrollRepo, EmployeeRepository empRepo)
        {
            _clockinRepo = clockinRepo;
            _payRunRepo = payrollRepo;
            _employeeRepo = empRepo;
        }

        public async Task<PayRunDto> GenerateRegularPayRun(string name, int month, int year, int createdBy)
        {
            var createdByName = await _employeeRepo.GetByIdAsync(createdBy);

            PayRunDto payRunDto = new PayRunDto
            {
                Name = name,
                PeriodMonth = month,
                PeriodYear = year,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                CreatedByName = createdByName == null ? "" : createdByName.EmpName,
                Status = "Pending",
                Type = "Monthly"
            };

            var employees = await _payRunRepo.GetEmployeesWithComponents(month, year);

            foreach (var employee in employees)
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
                            score += kpiCom.AssignedScore ??= kpiCom.SelfScore ??= 0 * kpiCom.Weight * 0.001m;
                        }
                        decimal kpiValue = score * ((kpi.Prorate != null && kpi.Prorate == true) ? actualClockinValue : expectedClockinValue);

                        if (kpiValue > 0)
                        {
                            var componentDto = new PayRunComponentDto
                            {
                                ComponentType = "Earning",
                                ComponentCode = "BONUS",
                                Description = "kpi",
                                Amount = kpiValue,
                                Taxable = true,
                                Insurable = true
                            };

                            itemDto.Components.Add(componentDto);
                            itemDto.GrossPay += actualClockinValue;
                        }
                    }
                }

                payRunDto.PayRunItems.Add(itemDto);
            }

            return payRunDto;
        }

        public async Task<bool> ContainsValidPayRunInPeriod(int month, int year)
        {
            return await _payRunRepo.ContainsValidPayRunInPeriod(month, year);
        }

        public async Task SaveRegularPayRun(PayRunDto run)
        {
            await _payRunRepo.SaveRegularPayRun(ToModel(run));
        }

        public async Task<IEnumerable<PayRunBasicDto>> GetAllAsync()
        {
            var runs = await _payRunRepo.GetAllAsync();
            return runs.Select(ToBasicDTO);
        }

        public async Task<PayRunDto?> GetPayRunAsync(int id)
        {
            var run = await _payRunRepo.GetDtoAsync(id);
            return run;
        }


        private PayRunBasicDto ToBasicDTO(PayRun run)
        {
            var dto = new PayRunBasicDto
            {
                PayrollRunId = run.PayrollRunId,
                Name = run.Name,
                PeriodMonth = run.PeriodMonth,
                PeriodYear = run.PeriodYear,
                Status = run.Status,
                CreatedDate = run.CreatedDate
            };

            return dto;
        }

        private PayRun ToModel(PayRunDto dto)
        {
            var model = new PayRun
            {
                PayrollRunId = dto.PayrollRunId,
                Name = dto.Name,
                PeriodMonth = dto.PeriodMonth,
                PeriodYear = dto.PeriodYear,
                Status = dto.Status,
                CreatedBy = dto.CreatedBy,
                CreatedDate = dto.CreatedDate,
                ApprovedFirstBy = dto.ApprovedFirstBy,
                ApprovedFirstAt = dto.ApprovedFirstAt,
                ApprovedFinalBy = dto.ApprovedFinalBy,
                ApprovedFinalAt = dto.ApprovedFinalAt,
                Type = dto.Type,
                PayRunItems = dto.PayRunItems?.Select(i => new PayRunItem
                {
                    PayRunItemId = i.ItemId,
                    EmpId = i.EmpId,
                    GrossPay = i.GrossPay,
                    Deductions = i.Deductions,
                    NetPay = i.NetPay,
                    Notes = i.Notes,
                    PayRunComponents = i.Components?.Select(c => new PayRunComponent
                    {
                        PayRunComponentId = c.ComponentId,
                        ComponentType = c.ComponentType,
                        ComponentCode = c.ComponentCode,
                        Description = c.Description,
                        Amount = c.Amount,
                        Taxable = c.Taxable,
                        Insurable = c.Insurable,
                        CreatedAt = c.CreatedAt
                    }).ToList() ?? new List<PayRunComponent>()
                }).ToList() ?? new List<PayRunItem>()
            };

            return model;
        }

        public async Task<bool> ApproveFirst(int approverId, int payRunId)
        {
            var approver = await _employeeRepo.GetByIdAsync(approverId);

            if (approver == null) { throw new Exception("Approver cannot be found!"); }

            var payRun = await _payRunRepo.FindAsync(payRunId);

            if (payRun == null) { throw new Exception("Pay run cannot be found!"); }

            payRun.ApprovedFirstBy = approverId;
            payRun.ApprovedFirstAt = DateTime.UtcNow;
            payRun.Status = "FirstApproved";

            await _payRunRepo.Update(payRun);

            return true;
        }

        public async Task<bool> ApproveFinal(int approverId, int payRunId)
        {
            var approver = await _employeeRepo.GetByIdAsync(approverId);

            if (approver == null) { throw new Exception("Approver cannot be found!"); }

            var payRun = await _payRunRepo.FindAsync(payRunId);

            if (payRun == null) { throw new Exception("Pay run cannot be found!"); }

            payRun.ApprovedFinalBy = approverId;
            payRun.ApprovedFinalAt = DateTime.UtcNow;
            payRun.Status = "FinalApproved";

            await _payRunRepo.Update(payRun);

            return true;
        }
    }
}
