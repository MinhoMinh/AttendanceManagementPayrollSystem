namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeSalaryPreviewDTO
    {
        public int EmpId { get; set; }
        public decimal BaseSalary { get; set; }
        public decimal GrossSalary { get; set; }
        public decimal TotalDeductions { get; set; }
        public decimal NetSalary { get; set; }
    }
}
