namespace AttendanceManagementPayrollSystem.DTO 
{
    public class LeaveRequestStatusDTO
    {
        public int ReqId { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? ApprovedBy { get; set; }
        public DateOnly? ApprovedDate { get; set; }
    }
}
