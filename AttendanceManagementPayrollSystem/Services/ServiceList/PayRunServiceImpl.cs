using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.Helper;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Http.HttpResults;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public class PayRunServiceImpl : PayRunService
    {
        private readonly PayRunRepository _payRunRepo;
        private readonly ClockinRepository _clockinRepo;
        private readonly EmployeeRepository _employeeRepo;
        private readonly IOvertimeRepository _overtimeRepo;
        private readonly HolidayCalendarRepository _holidayRepo;
        private readonly SalaryPolicyRepository _salaryPolicyRepo;
        private readonly TaxRepository _taxRepo;
        private readonly InsuranceRateRepository _insuranceRepo;
        private readonly ILeaveRequestRepository _leaveRepo;

        private readonly ShiftService _shiftService;
        private readonly BonusService _bonusService;

        public PayRunServiceImpl(ShiftService shiftService, SalaryPolicyRepository salaryPolicyRepo, ClockinRepository clockinRepo,
            PayRunRepository payrollRepo, EmployeeRepository empRepo, HolidayCalendarRepository holidayRepo, 
            IOvertimeRepository overtimeRepo, TaxRepository taxRepo, InsuranceRateRepository insuranceRepo,
            BonusService bonusService, ILeaveRequestRepository leaveRepo)
        {
            _salaryPolicyRepo = salaryPolicyRepo;
            _shiftService = shiftService;
            _bonusService = bonusService;
            _clockinRepo = clockinRepo;
            _overtimeRepo = overtimeRepo;
            _payRunRepo = payrollRepo;
            _employeeRepo = empRepo;
            _holidayRepo = holidayRepo;
            _overtimeRepo = overtimeRepo;
            _taxRepo = taxRepo;
            _insuranceRepo = insuranceRepo;
            _leaveRepo = leaveRepo;
        }

        public async Task<PayRunContext> GetPayRunContext(int month, int year) 
        {
            Console.WriteLine($"{month} {year}");
            var periodStart = new DateTime(year, month, 1);
            var periodEnd = periodStart.AddMonths(1).AddDays(-1);

            var salaryPolicy = await _salaryPolicyRepo.GetActiveSalaryPolicyAsync();

            var holidays = await _holidayRepo.GetByRangeAsync(periodStart, periodEnd);

            var tax = await _taxRepo.GetActiveTaxInTime(periodStart, periodEnd);

            var insurances = await _insuranceRepo.GetActiveInsurancesInTime(periodStart, periodEnd);

            var socialInsurance = insurances.FirstOrDefault(x => x.Category == "Society");
            var healthInsurance = insurances.FirstOrDefault(x => x.Category == "Health");
            var unemployeeInsurance = insurances.FirstOrDefault(x => x.Category == "Unemployee");


            return new PayRunContext(salaryPolicy, tax, socialInsurance, healthInsurance, unemployeeInsurance, holidays,
                periodStart, periodEnd);
        }

        public async Task<PayRunDto> GenerateRegularPayRun(string name, int month, int year, int createdBy)
        {
            var createdByName = await _employeeRepo.GetByIdAsync(createdBy);

            var context = await GetPayRunContext(month, year);

            PayRunDto payRunDto = new PayRunDto
            {
                Name = name,
                PeriodMonth = month,
                PeriodYear = year,
                CreatedDate = DateTime.Now,
                CreatedBy = createdBy,
                CreatedByName = createdByName == null ? "" : createdByName.EmpName,
                Status = "Pending",
                Type = "Monthly",
                TaxId = context.Tax.TaxId,
                SocialInsuranceId = context.SocialInsurance.RateSetId,
                HealthInsuranceId = context.HealthInsurance.RateSetId,
                UnemployeeInsuranceId = context.UnemployeeInsurance.RateSetId,
                SalaryPolicyId = context.SalaryPolicy.SalId
            };


            var periodStart = context.PeriodStart;
            var periodEnd = context.PeriodEnd;

            //var salaryPolicy = await _salaryPolicyRepo.GetActiveSalaryPolicyAsync();

            var employees = await _payRunRepo.GetEmployeesWithComponents(month, year);

            var overtimeDict = await _overtimeRepo.GetApprovedOvertimes(DateOnly.FromDateTime(periodStart), DateOnly.FromDateTime(periodEnd));

            var shiftDictionary = await _shiftService.GetWeeklyShiftDtos(employees.Select(e => e.EmpId));

            var bonus = await _bonusService.GetBonusByTime(DateOnly.FromDateTime(periodStart), DateOnly.FromDateTime(periodEnd));

            var leaves = await _leaveRepo.GetApprovedLeaveByDate(DateOnly.FromDateTime(periodStart), DateOnly.FromDateTime(periodEnd));

            var holidays = context.Holidays;


            foreach (var employee in employees)
            {

                // find shift for employee
                shiftDictionary.TryGetValue(employee.EmpId, out var shift);

                overtimeDict.TryGetValue(employee.EmpId, out var overtimes);

                var empBonus = bonus
                    .Where(b => b.EmpBonus.Any(e => e.EmpId == employee.EmpId))
                    .ToList();

                var empLeave = leaves
                    .Where(l => l.EmpId == employee.EmpId).ToList();

                var payItem = PayRunCalculator.CalculatePay(context, employee, shift, overtimes, empBonus, empLeave);
                payRunDto.PayRunItems.Add(payItem);
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

        public async Task<List<PayRunPreviewDTO>> GetPayRunByEmpIdAndDateAsync(int empId, int periodMonth, int periodYear)
        {
            return await _payRunRepo.GetPayRunByEmpIdAndDateAsync(empId, periodMonth, periodYear);

        }

        public async Task<List<PayRunPreviewDTO>?> GetPayRunsForEmployeeAsync(
        int empId, DateTime start, DateTime end)
        {
            return await _payRunRepo.GetPayRunsForEmployeeAsync(empId, start, end);
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
                RejectedBy = dto.RejectedBy,
                RejectedAt = dto.RejectedAt,
                Type = dto.Type,
                TaxId = dto.TaxId,
                SocialInsuranceId = dto.SocialInsuranceId,
                HealthInsuranceId = dto.HealthInsuranceId,
                UnemployeeInsuranceId = dto.UnemployeeInsuranceId,
                SalaryPolicyId = dto.SalaryPolicyId,
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

        public async Task<bool> Reject(int approverId, int payRunId)
        {
            var approver = await _employeeRepo.GetByIdAsync(approverId);

            if (approver == null) { throw new Exception("Approver cannot be found!"); }

            var payRun = await _payRunRepo.FindAsync(payRunId);

            if (payRun == null) { throw new Exception("Pay run cannot be found!"); }

            payRun.RejectedBy = approverId;
            payRun.RejectedAt = DateTime.UtcNow;
            payRun.Status = "Void";

            await _payRunRepo.Update(payRun);

            return true;
        }
    }
}
