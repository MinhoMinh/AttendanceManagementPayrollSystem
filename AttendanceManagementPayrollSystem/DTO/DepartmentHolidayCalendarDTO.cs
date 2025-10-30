namespace AttendanceManagementPayrollSystem.DTO
{
    public class DepartmentHolidayCalendarDTO
    {
        public int DepHolidayCalendarId { get; set; }
        public int? DepId { get; set; }
        public int? HolidayId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Thông tin mở rộng (để hiển thị)
        public string DepName { get; set; }
        public string HolidayName { get; set; }
    }
}
