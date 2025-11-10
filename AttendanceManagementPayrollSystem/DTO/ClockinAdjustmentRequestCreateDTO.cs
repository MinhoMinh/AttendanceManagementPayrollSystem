namespace AttendanceManagementPayrollSystem.DTO
{
    public class ClockinAdjustmentRequestCreateDTO
    {
        public int EmployeeId { get; set; }

        public int ClockInComponentId { get; set; }

        public decimal RequestedValue { get; set; }

        public string Message { get; set; }

        public IFormFile? Attachment { get; set; }
    }
}
