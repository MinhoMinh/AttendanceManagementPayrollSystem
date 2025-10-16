using System;
namespace AttendanceManagementPayrollSystem.DTO
{
    public class DailyDetailDTO
    {
        public int Day { get; set; } 
        public List<string> Logs { get; set; } = new(); // Giờ vào/ra trong ngày
        public List<decimal> Actual { get; set; } = new();         // Tổng công thực tế
        public List<decimal> Scheduled { get; set; } = new();        // Tổng công dự kiến
    }
}
