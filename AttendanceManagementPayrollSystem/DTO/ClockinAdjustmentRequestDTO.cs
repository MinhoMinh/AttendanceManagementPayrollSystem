namespace AttendanceManagementPayrollSystem.DTO
{
    public class ClockinAdjustmentRequestDTO
    {
        public int RequestId { get; set; }

        public int EmployeeId { get; set; }

        public int ClockInComponentId { get; set; }

        public decimal RequestedValue { get; set; }

        public string Message { get; set; }

        public string Attachment { get; set; }

        public string Status { get; set; }

        public int? ApproverId { get; set; }

        public string Comment { get; set; }

        public DateTime? CreatedAt { get; set; }

        public string EmployeeName { get; set; }

        public string ApproverName { get; set; }
    }
}
