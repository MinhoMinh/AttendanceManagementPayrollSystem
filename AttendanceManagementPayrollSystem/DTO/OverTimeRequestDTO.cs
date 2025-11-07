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
        public int? OvertimeType { get; set; }
        public string OvertimeTypeName { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string ApprovedByName { get; set; }
        public int? ApprovedBy { get; set; }
        public DateOnly? ApprovedDate { get; set; }
        public int? LinkedPayRunId { get; set; }
        public string LinkedPayRunName { get; set; }
        public DateOnly? CreatedDate { get; set; }
    }

    public class OvertimeRateDTO
    {
        public int Id { get; set; }
        public string OvertimeType { get; set; }
        public decimal RateMultiplier { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public int? CreatedBy { get; set; }
    }

}

