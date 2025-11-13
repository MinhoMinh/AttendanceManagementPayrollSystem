namespace AttendanceManagementPayrollSystem.DTO
{
    public class DailyShiftCreateDTO
    {
        public string ShiftName { get; set; }

        public string ShiftDescription { get; set; }

        // Danh sách các khung giờ trong ca làm việc
        public List<ShiftSegmentDTO> Segments { get; set; } = new();
    }

    public class ShiftSegmentDTO
    {
        public TimeSpan StartTime { get; set; }  // giờ vào
        public TimeSpan EndTime { get; set; }    // giờ ra
        public decimal WorkUnit { get; set; }    // công
    }
}
