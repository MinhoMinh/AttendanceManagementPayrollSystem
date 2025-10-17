namespace AttendanceManagementPayrollSystem.DTO
{
    public class HolidayCalendarDTO
    {
        public int HolidayId { get; set; }
        public string HolidayName { get; set; }
        public DateTime StartDatetime { get; set; }
        public DateTime EndDatetime { get; set; }
        public DateTime? CreatedAt { get; set; }
        //public ICollection<DepartmentDTO> Deps { get; set; }

    }
}
