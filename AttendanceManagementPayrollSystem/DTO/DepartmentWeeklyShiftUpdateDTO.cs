namespace AttendanceManagementPayrollSystem.DTO
{
    public class DepartmentWeeklyShiftUpdateDTO
    {
        public int DeptShiftId { get; set; }  // cần biết bản ghi nào đang update

        public int DepId { get; set; }       // có thể cho phép đổi phòng ban
        public int ShiftId { get; set; }     // đổi ca nếu cần
        public bool IsActive { get; set; }   // đổi trạng thái
    }
}
