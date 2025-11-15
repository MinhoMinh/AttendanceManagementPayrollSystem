using System.Runtime.InteropServices;

namespace AttendanceManagementPayrollSystem.DTOs
{
    public class LeaveRequestDTO
    {
        public int ReqId { get; set; }
        public int EmpId { get; set; }
        public int TypeId { get; set; }
        public DateOnly ReqDate { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal NumbersOfDays { get; set; }
        public string Status { get; set; } = "Pending";
        public string Reason { get; set; }     // lý do
        public int? ApprovedBy { get; set; }
        public DateOnly? ApprovedDate { get; set; }

        public string ApprovedByName { get; set; }

    }
}

