namespace AttendanceManagementPayrollSystem.DTO
{
    public class PayRunPreviewDTO
    {
        public int PayrollRunId { get; set; }
        public string Name { get; set; }
        public int PeriodMonth { get; set; }
        public int PeriodYear { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Type { get; set; }

        public List<PayRunItemDto> PayRunItems { get; set; } = new();
    }
}
