using System.Runtime.InteropServices;

namespace AttendanceManagementPayrollSystem.DTOs
{
    public class LeaveRequestDTO
    {
        public int ReqId { get; set; }
        public int EmpId { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public int TypeId { get; set; }
        public string TypeName { get; set; } = string.Empty;
        public string Status { get; set; } = "Pending";
        public string Reason { get; set; }     // lý do
        public string Details { get; set; }    // chi tiết
    }
}

