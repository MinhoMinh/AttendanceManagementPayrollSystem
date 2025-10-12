using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeAllowanceDTO
    {
        public int Id { get; set; }

        public int EmpId { get; set; }

        public string EmpName { get; set; }

        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public decimal? CustomValue { get; set; }

        public DateOnly StartDate { get; set; }

        public DateOnly? EndDate { get; set; }

        public string Status { get; set; }

        public int CreatedBy { get; set; }

        public string CreatedByName { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
