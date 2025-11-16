namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeAllowanceCreateDTO
    {
        public int Id { get; set; }
        public int EmpId { get; set; }
        public int TypeId { get; set; }
        public decimal? CustomValue { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public string Status { get; set; }
        public int CreatedBy { get; set; }
    }
}