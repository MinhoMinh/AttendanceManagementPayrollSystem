namespace AttendanceManagementPayrollSystem.DTO
{
    public class TaxBracketDTO
    {
        public int BracketId { get; set; }
        public decimal LowerBound { get; set; }
        public decimal? UpperBound { get; set; }
        public decimal Rate { get; set; }
        public int? TaxId { get; set; }
    }
}
