using System;
namespace AttendanceManagementPayrollSystem.DTO
{
    public class DailyDetailDTO
    {
        public int Day { get; set; } 
        public List<string> Logs { get; set; } = new(); // Giờ vào/ra trong ngày
        public decimal? ActualUnit { get; set; }         // Tổng công thực tế
        public decimal? ScheduledUnit { get; set; }       // Tổng công dự kiến
        public decimal? ActualHour { get; set; }
        public decimal? ScheduledHour { get; set; }
    }
}
