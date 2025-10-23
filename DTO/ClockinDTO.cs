namespace AttendanceManagementPayrollSystem.DTO
{
    public class ClockinDTO
    {
        public int CloId { get; set; }

        public int EmpId { get; set; }

        public DateTime Date { get; set; }

        public decimal? WorkUnits { get; set; }

        public decimal? ScheduledUnits { get; set; }

        public decimal? WorkHours { get; set; }

        public decimal? ScheduledHours { get; set; }

        public string ClockLog { get; set; }

        public string WorkUnitBreakdown { get; set; }

        public List<DailyDetailDTO> DailyRecords { get; set; } = new();

        public List<ClockinComponentDto> Components { get; set; } = new();
    }

    public class ClockinComponentDto
    {
        public int Id { get; set; }

        public string Shift { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? CheckIn { get; set; }

        public DateTime? CheckOut { get; set; }

        public string ClockinLog { get; set; }

        public string Description { get; set; }

        public decimal? WorkHours { get; set; }

        public decimal? ScheduledHours { get; set; }

        public decimal? WorkUnits { get; set; }

        public decimal? ScheduledUnits { get; set; }
    }
}
