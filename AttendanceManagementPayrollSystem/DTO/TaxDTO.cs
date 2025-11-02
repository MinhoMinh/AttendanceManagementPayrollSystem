namespace AttendanceManagementPayrollSystem.DTO
{
    public class TaxDTO
    {
        public int TaxId { get; set; }
        public string TaxName { get; set; }
        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }

        public bool IsActive { get; set; }

        public List<TaxBracketDTO>? TaxBrackets { get; set; }
    }
}
