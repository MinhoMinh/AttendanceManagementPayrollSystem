using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DTO
{
    public class PayRunItemDTO
    {
        public int PayRunItemId { get; set; }

        public int PayRunId { get; set; }

        public int EmpId { get; set; }

        public decimal GrossPay { get; set; }

        public decimal Deductions { get; set; }

        public decimal NetPay { get; set; }

        public string Notes { get; set; }

        public virtual Employee Emp { get; set; }

        public virtual PayRun PayRun { get; set; }

        public List<PayRunComponentDTO> PayRunComponents{ get; set; } = new ();
    }
}
