namespace AttendanceManagementPayrollSystem.DTO
{
    public class WeeklyShiftViewDTO
    {
        public int ShiftId { get; set; }

        public string ShiftName { get; set; }

        public DailyShiftViewDTO? MonDailyShift { get; set; }

        public DailyShiftViewDTO? TueDailyShift { get; set; }

        public DailyShiftViewDTO? WedDailyShift { get; set; }

        public DailyShiftViewDTO? ThuDailyShift { get; set; }

        public DailyShiftViewDTO? FriDailyShift { get; set; }

        public DailyShiftViewDTO? SatDailyShift { get; set; }

        public DailyShiftViewDTO? SunDailyShift { get; set; }

        public string ShiftDescription { get; set; }

        public DailyShiftViewDTO? GetDailyShift(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Sunday:
                    return SunDailyShift;
                case DayOfWeek.Monday:
                    return MonDailyShift;
                case DayOfWeek.Tuesday:
                    return TueDailyShift;
                case DayOfWeek.Wednesday:
                    return WedDailyShift;
                case DayOfWeek.Thursday:
                    return ThuDailyShift;
                case DayOfWeek.Friday:
                    return FriDailyShift;
                case DayOfWeek.Saturday:
                    return SatDailyShift;
                default: return null;
            }
        }
    }
}
