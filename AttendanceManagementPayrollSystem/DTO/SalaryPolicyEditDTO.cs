namespace AttendanceManagementPayrollSystem.DTO
{
    public class SalaryPolicyEditDTO
    {
        public int SalId { get; set; }

        public string SalaryPolicyName { get; set; }

        public decimal WorkUnitValue { get; set; }

        public int YearlyPto { get; set; }

        public decimal OverclockMultiplier { get; set; }

        public DateTime EffectiveFrom { get; set; }

        public bool IsActive { get; set; }
    }
}
