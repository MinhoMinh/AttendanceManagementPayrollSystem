using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DTO
{
    public class PayRunComponentDTO
    {
        public int PayRunComponentId { get; set; }

        public int PayRunItemId { get; set; }

        public string ComponentType { get; set; }

        public string ComponentCode { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }

        public bool Taxable { get; set; }

        public DateTime? CreatedAt { get; set; }

        public bool Insurable { get; set; }

    }
}