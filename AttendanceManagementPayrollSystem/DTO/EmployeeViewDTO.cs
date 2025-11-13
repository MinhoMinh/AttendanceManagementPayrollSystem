namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeViewDTO
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public DateOnly? EmpDob { get; set; }
        public string EmpPhoneNumber { get; set; }
        public int? DepId { get; set; }
        public string? DepName { get; set; }
        public string Username { get; set; }

        // Balance
        public BalanceDTO Pto { get; set; }
        public BalanceDTO Sick { get; set; }
        public BalanceDTO Vacation { get; set; }
        public BalanceDTO Overtime { get; set; }
        public DateTime? LastUpdated { get; set; }
    }

    public class BalanceDTO
    {
        public double Remaining { get; set; }
        public double Used { get; set; }
        public double Total { get; set; }
    }
}
