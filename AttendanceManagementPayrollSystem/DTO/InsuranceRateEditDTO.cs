namespace AttendanceManagementPayrollSystem.DTO
{
    public class InsuranceRateEditDTO
    {
        public int RateSetId { get; set; }

        public string TypeName { get; set; }

        public decimal EmployeeRate { get; set; }

        public decimal EmployerRate { get; set; }

        public string CapRule { get; set; }

        public DateOnly EffectiveFrom { get; set; }

        public DateOnly? EffectiveTo { get; set; }

        public string LawCode { get; set; }

        public bool IsActive { get; set; }
    }
}
