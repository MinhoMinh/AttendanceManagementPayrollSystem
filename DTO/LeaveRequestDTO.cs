namespace AttendanceManagementPayrollSystem.DTOs
{
    public class LeaveRequestDTO
    {
        public int EmpId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public string LeaveType { get; set; }  // loại nghỉ
        public string Reason { get; set; }     // lý do
        public string Details { get; set; }    // chi tiết
    }
}

