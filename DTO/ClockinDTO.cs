namespace AttendanceManagementPayrollSystem.DTO
{
    public class ClockinDTO
    {
        public int CloId { get; set; }

        public int EmpId { get; set; }

        public DateTime Date { get; set; }

        public decimal? WorkUnits { get; set; }

        public decimal? ScheduledUnits { get; set; }

        public string ClockLog { get; set; }

        public string WorkUnitBreakdown { get; set; }

        public List<DailyDetailDTO> DailyRecords { get; set; } = new();
    }
}
