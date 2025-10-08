namespace AttendanceManagementPayrollSystem.DTO
{
    public class PayrollRunDTO
    {
        public int PayrollRunId { get; set; }
        public string Name { get; set; }
        public int PeriodMonth { get; set; }
        public int PeriodYear { get; set; }
        public string Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ApprovedFirstBy { get; set; }
        public int? ApprovedFinalBy { get; set; }


        public List<EmployeeSalaryPreviewDTO> Previews { get; set; } = new();
    }
}
