namespace AttendanceManagementPayrollSystem.DTO
{
    public class ClockinAdjustmentRespondDTO
    {
        public int RequestId { get; set; }
        public int ApproverId { get; set; }
        public string status { get; set; } = string.Empty;
        public string? Comment { get; set; }
    }
}
