namespace AttendanceManagementPayrollSystem.DTO
{
    public class ClockinAdjustmentApprovalDTO
    {
        public int RequestId { get; set; }
        public string Status { get; set; }  // Approved / Denied
        public string Comment { get; set; }
        public int ApproverId { get; set; }
    }
}
