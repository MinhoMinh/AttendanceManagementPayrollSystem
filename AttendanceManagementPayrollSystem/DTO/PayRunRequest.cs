namespace AttendanceManagementPayrollSystem.DTO
{
    public class PayRunRequest
    {
        public string Name { get; set; }
        public int PeriodMonth { get; set; }
        public int PeriodYear { get; set; }
        public int CreatedBy { get; set; }
    }
}
