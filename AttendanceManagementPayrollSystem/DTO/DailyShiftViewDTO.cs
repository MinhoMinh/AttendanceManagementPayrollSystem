namespace AttendanceManagementPayrollSystem.DTO
{
    public class DailyShiftViewDTO
    {
        public int ShiftId { get; set; }
        public string ShiftName { get; set; }
        public string ShiftDescription { get; set; }
        public string ShiftString { get; set; }

        // Danh sách các segment, mỗi segment có Start/End, công và số giờ
        public List<ShiftDto> Segments { get; set; } = new();

        // Tổng công của ca
        public decimal? ShiftWorkUnit { get; set; }
    }
}
