using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.InkML;

namespace AttendanceManagementPayrollSystem.Services.Helper
{
    public class PayRunContext
    {
        public PayRunContext(
            SalaryPolicy salaryPolicy,
            Tax tax,
            InsuranceRate socialInsurance,
            InsuranceRate healthInsurance,
            InsuranceRate unemployeeInsurance,
            IEnumerable<HolidayCalendarDTO> holidays,
            DateTime periodStart, DateTime periodEnd) 
        {
            this.SalaryPolicy = salaryPolicy;
            this.Tax = tax;
            this.SocialInsurance = socialInsurance;
            this.HealthInsurance = healthInsurance;
            this.UnemployeeInsurance = unemployeeInsurance;
            this.Holidays = holidays;
            this.PeriodStart = periodStart;
            this.PeriodEnd = periodEnd;
        }

        public SalaryPolicy SalaryPolicy { get; set; }
        public Tax Tax { get; set; }
        public InsuranceRate SocialInsurance { get; set; }
        public InsuranceRate HealthInsurance { get; set; }
        public InsuranceRate UnemployeeInsurance { get; set; }
        //public Employee Employee { get; private set; }
        //public WeeklyShiftDto Shift { get; private set; }
        //public List<OvertimeRequest> Overtimes { get; private set; }
        public IEnumerable<HolidayCalendarDTO> Holidays { get; private set; }
        public DateTime PeriodStart { get; private set; }
        public DateTime PeriodEnd { get; private set; }

        private bool IsValidContext =>
            SalaryPolicy != null
            && Tax != null
        && SocialInsurance != null
            && HealthInsurance != null
        && UnemployeeInsurance != null;
    }
}
