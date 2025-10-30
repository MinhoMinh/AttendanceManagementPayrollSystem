using System;

namespace AttendanceManagementPayrollSystem.DTO
{
    public class OvertimeRequestDTO
    {
        public int ReqId { get; set; }
        public DateOnly ReqDate { get; set; }
        public TimeOnly? StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public decimal? Hours { get; set; }
        public string OvertimeType { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string ApprovedByName { get; set; }
        public DateOnly? ApprovedDate { get; set; }
    }
}

