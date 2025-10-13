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

            var policy = await _payrollRepo.GetActivePolicyAsync(periodMonth, periodYear);
            if (policy == null)
                throw new Exception("No active salary policy found for this period.");


            foreach (var c in clockins)
            {

                //decimal hourlyRate = emp.HourlyRate ?? 0;
                decimal basePay = (c.WorkUnits ?? 0) * policy.WorkUnitValue;
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


            return ToDTO(savedRun);
        }

        public async Task<PayrollRunDTO> ApproveFirstAsync(int payrollId, int approvedBy)
        {
            var payroll = await _payrollRepo.FindAsync(payrollId);
            if (payroll == null)
                throw new Exception("Payroll not found.");

            // Example logic
            if (payroll.ApprovedFirstBy != null)
                throw new Exception("Already first-approved.");

            payroll.ApprovedFirstBy = approvedBy;
            payroll.Status = "FirstApproved";
            payroll.ApprovedFirstAt = DateTime.Now;


            await _payrollRepo.Update(payroll);

            return ToDTO(payroll);
        }

        public async Task<PayrollRunDTO> ApproveFinalAsync(int payrollId, int approvedBy)
        {
            var payroll = await _payrollRepo.FindAsync(payrollId);
            if (payroll == null)
                throw new Exception("Payroll not found.");

            // Example logic
            if (payroll.ApprovedFinalBy != null)
                throw new Exception("Already first-approved.");

            payroll.ApprovedFinalBy = approvedBy;
            payroll.Status = "FinalApproved";
            payroll.ApprovedFinalAt = DateTime.Now;

            await _payrollRepo.Update(payroll);

            return ToDTO(payroll);
        }

        public async Task<PayrollRunDTO> RejectAsync(int payrollId, int rejectedBy)
        {
            var payroll = await _payrollRepo.FindAsync(payrollId);
            if (payroll == null)
                throw new Exception("Payroll not found.");

            payroll.Status = "Rejected";
            //payroll.RejectedBy = rejectedBy;
            await _payrollRepo.Update(payroll);

            return ToDTO(payroll);
        }

        public async Task<IEnumerable<PayrollRunDTO>> GetAllAsync()
        {
            var runs = await _payrollRepo.GetAllAsync();
            return runs.Select(ToDTO);
        }


        private PayrollRunDTO ToDTO(PayrollRun savedRun)
        {
            var dto = new PayrollRunDTO
            {
                PayrollRunId = savedRun.PayrollRunId,
                Name = savedRun.Name,
                PeriodMonth = savedRun.PeriodMonth,
                PeriodYear = savedRun.PeriodYear,
                Status = savedRun.Status,
                CreatedDate = savedRun.CreatedDate,
                ApprovedFirstBy = savedRun.ApprovedFirstBy,
                ApprovedFinalBy = savedRun.ApprovedFinalBy,
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
