namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeInDepartmentDTO
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpPhoneNumber { get; set; }
        public bool IsActive { get; set; }
    }
    public class EmployeeGroupByDepartmentDTO
    {
        public int DepId { get; set; }
        public string DepName { get; set; }
        public List<EmployeeInDepartmentDTO> Employees { get; set; } = new();
    }

}
