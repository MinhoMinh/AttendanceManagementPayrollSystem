namespace AttendanceManagementPayrollSystem.DTO
{
    public class TaxBracketEditDTO
    {
        public int? BracketId { get; set; } // Nullable nếu thêm mới
        public decimal LowerBound { get; set; }
        public decimal? UpperBound { get; set; }
        public decimal Rate { get; set; }
        public int? TaxId { get; set; } // Nullable nếu thêm mới
    }
}
