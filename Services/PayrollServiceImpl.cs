using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public class PayrollServiceImpl : PayrollService
    {
        private readonly ClockinRepository _clockinRepo;
        private readonly PayrollRepository _payrollRepo;

        public PayrollServiceImpl(ClockinRepository clockinRepo, PayrollRepository payrollRepo)
        {
            _clockinRepo = clockinRepo;
            _payrollRepo = payrollRepo;
        }

        public async Task<PayrollRunDTO> GeneratePayrollAsync(string name, int periodMonth, int periodYear, int createdBy)
        {
            var run = new PayrollRun
            {
                Name = name,
                PeriodMonth = periodMonth,
                PeriodYear = periodYear,
                CreatedBy = createdBy,
                CreatedDate = DateTime.Now,
                Status = "Pending",
                EmployeeSalaryPreviews = new List<EmployeeSalaryPreview>()
            };

            var clockins = await _clockinRepo.GetByDateRangeAsync(periodMonth, periodYear);


            foreach (var c in clockins)
            {


                //decimal hourlyRate = emp.HourlyRate ?? 0;
                decimal basePay = (c.WorkUnits ?? 0) * 200000;
                //decimal deductions = 0; // maybe from Leave or Penalty tables
                //decimal allowances = 0; // maybe from Bonus or Allowance tables
                //decimal netPay = basePay + allowances - deductions;

                run.EmployeeSalaryPreviews.Add( new EmployeeSalaryPreview
                {
                    EmpId = c.EmpId,
                    BaseSalary = basePay,
                    GrossSalary = basePay,
                    TotalDeductions = 0,
                    NetSalary = basePay,
                    CreatedBy = createdBy,
                    InsuranceRateSetId = 1
                });
            }

            var savedRun = await _payrollRepo.AddAsync(run);
            var dto = new PayrollRunDTO
            {
                PayrollRunId = savedRun.PayrollRunId,
                Name = savedRun.Name,
                PeriodMonth = savedRun.PeriodMonth,
                PeriodYear = savedRun.PeriodYear,
                Status = savedRun.Status,
                CreatedDate = savedRun.CreatedDate,
                Previews = savedRun.EmployeeSalaryPreviews
            .Select(p => new EmployeeSalaryPreviewDTO
            {
                EmpId = p.EmpId,
                BaseSalary = p.BaseSalary,
                GrossSalary = p.GrossSalary,
                TotalDeductions = p.TotalDeductions,
                NetSalary = p.NetSalary
            }).ToList()
            };

            return dto;

        }
    }
}
