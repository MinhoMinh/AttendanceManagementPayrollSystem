namespace AttendanceManagementPayrollSystem.DTO
{
    public class WeeklyShiftDto
    {
        public int ShiftId { get; set; }

        public string ShiftName { get; set; }

        public DailyShiftDto? MonDailyShift { get; set; }

        public DailyShiftDto? TueDailyShift { get; set; }

        public DailyShiftDto? WedDailyShift { get; set; }

        public DailyShiftDto? ThuDailyShift { get; set; }

        public DailyShiftDto? FriDailyShift { get; set; }

        public DailyShiftDto? SatDailyShift { get; set; }

        public DailyShiftDto? SunDailyShift { get; set; }

        public string ShiftDescription { get; set; }

        public DailyShiftDto? GetDailyShift(DayOfWeek dayOfWeek)
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
    public class DailyShiftDto
    {
        public List<ShiftDto> ShiftDtos { get; set; } = new();
    }

    public class ShiftDto
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public decimal WorkUnits { get; set; }
        public decimal WorkHours { get; set; }

        public override string ToString()
        {
            return $"{StartTime:hh\\:mm} - {EndTime:hh\\:mm}"; ;
        }
    }
    
}
