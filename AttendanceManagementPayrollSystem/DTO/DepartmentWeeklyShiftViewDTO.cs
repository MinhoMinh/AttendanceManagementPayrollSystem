namespace AttendanceManagementPayrollSystem.DTO
{
    public class DepartmentWeeklyShiftViewDTO
    {
        public int DeptShiftId { get; set; }

        public int DepId { get; set; }
        public string DepName { get; set; }  // tên phòng ban

        public int ShiftId { get; set; }
        public string ShiftName { get; set; } // tên ca

        public bool IsActive { get; set; }
    }
}
