using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DTO
{
    public class AllowanceTypeDTO
    {
        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public string CalculationType { get; set; }

        public decimal? Value { get; set; }

        public DateOnly EffectiveStartDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public virtual ICollection<EmployeeAllowance> EmployeeAllowances { get; set; } = new List<EmployeeAllowance>();
    }
}