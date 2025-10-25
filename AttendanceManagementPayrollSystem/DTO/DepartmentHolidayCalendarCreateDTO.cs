namespace AttendanceManagementPayrollSystem.DTO
{
    public class DepartmentHolidayCalendarCreateDTO
    {
        public int DepId { get; set; }
        public int HolidayId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

}
