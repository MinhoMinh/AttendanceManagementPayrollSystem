namespace AttendanceManagementPayrollSystem.DTO
{
    public class PayrollRequest
    {
        public string Name { get; set; }
        public int PeriodMonth { get; set; }
        public int PeriodYear { get; set; }
        public int CreatedBy { get; set; }
    }
}
