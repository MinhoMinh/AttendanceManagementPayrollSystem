namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeCreateDTO
    {
        public string EmpName { get; set; }
        public DateOnly? EmpDob { get; set; }
        public string EmpPhoneNumber { get; set; }
        public int? DepId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // plain text, service sẽ hash
    }
}
