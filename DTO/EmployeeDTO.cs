namespace AttendanceManagementPayrollSystem.DTO
{
	public class EmployeeDTO
	{
		public int EmpId { get; set; }
		public string EmpName { get; set; }
		public string EmpRole { get; set; }
		public List<string> Permissions { get; set; } = new List<string>();
	}

}