namespace AttendanceManagementPayrollSystem.DTO
{
    public class LeaveRequestApprovalDTO
    {
        public int ReqId { get; set; }
        public string Status { get; set; }
        public int? ApprovedBy { get; set; }
        public DateOnly? ApprovedDate { get; set; }
    }
}
