using System;
namespace AttendanceManagementPayrollSystem.DTO
{
    public class DailyDetailDTO
    {
        public int Day { get; set; } 
        public List<string> Logs { get; set; } = new(); // Giờ vào/ra trong ngày
        public decimal Actual { get; set; }            // Tổng công thực tế
        public decimal Scheduled { get; set; }         // Tổng công dự kiến
    }
}
