namespace AttendanceManagementPayrollSystem.DTO
{
    public class DepartmentAttendanceDTO
    {
        public string DepartmentName { get; set; }
        public List<int> EmployeeIds { get; set; } = new();
        public int TotalEmployees { get; set; }
        public decimal TotalWorkHours { get; set; }
        public decimal TotalScheduledHours { get; set; }
        public int AbsentDays { get; set; }
        public decimal AttendanceRate
        {
            get
            {
                return TotalScheduledHours == 0 ? 0 :
                    Math.Round((TotalWorkHours / TotalScheduledHours) * 100, 2);
            }
        }
    }
}
