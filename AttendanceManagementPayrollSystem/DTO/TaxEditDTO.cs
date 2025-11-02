namespace AttendanceManagementPayrollSystem.DTO
{
    public class TaxEditDTO
    {
        public int? TaxId { get; set; } // Nullable để tạo mới
        public string TaxName { get; set; }
        public DateOnly EffectiveFrom { get; set; }
        public DateOnly? EffectiveTo { get; set; }
        public bool IsActive { get; set; }

        public List<TaxBracketEditDTO> TaxBrackets { get; set; } = new List<TaxBracketEditDTO>();
    }
}
