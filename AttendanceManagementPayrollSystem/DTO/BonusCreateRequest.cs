namespace AttendanceManagementPayrollSystem.DTO
{
    public class BonusCreateRequest
    {
        public string BonusName { get; set; } = string.Empty;
        public decimal BonusAmount { get; set; }
        public DateTime BonusPeriod { get; set; }  // hoặc DateOnly nếu bạn map theo kiểu đó trong EF
    }
}
