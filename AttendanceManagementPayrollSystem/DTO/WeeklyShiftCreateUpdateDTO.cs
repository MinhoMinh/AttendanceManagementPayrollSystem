namespace AttendanceManagementPayrollSystem.DTO
{
    public class WeeklyShiftCreateUpdateDTO
    {
        public string ShiftName { get; set; }
        public string? ShiftDescription { get; set; }

        public int? MonDailyShiftId { get; set; }
        public int? TueDailyShiftId { get; set; }
        public int? WedDailyShiftId { get; set; }
        public int? ThuDailyShiftId { get; set; }
        public int? FriDailyShiftId { get; set; }
        public int? SatDailyShiftId { get; set; }
        public int? SunDailyShiftId { get; set; }
    }
}
