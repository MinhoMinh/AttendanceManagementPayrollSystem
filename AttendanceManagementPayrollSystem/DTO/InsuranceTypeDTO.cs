namespace AttendanceManagementPayrollSystem.DTO
{
    public class InsuranceTypeDTO
    {
        public int TypeId { get; set; }

        public string TypeName { get; set; }

        public string LawName { get; set; }

        public string LawCode { get; set; }

        public string Description { get; set; }

        public DateOnly EffectiveFrom { get; set; }

        public DateOnly? EffectiveTo { get; set; }

    }
}
