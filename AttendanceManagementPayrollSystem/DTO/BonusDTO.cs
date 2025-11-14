namespace AttendanceManagementPayrollSystem.DTO
{
    public class BonusDTO
    {
        public int BonusId { get; set; }
        public string BonusName { get; set; }
        public decimal BonusAmount { get; set; }
        public DateOnly? BonusPeriod { get; set; }
    }

    public class AssignBonusRequest
    {
        public List<int> EmpIds { get; set; } = new();
        public int BonusId { get; set; }
        public decimal? OverrideAmount { get; set; }
        public int PeriodMonth { get; set; }
        public int PeriodYear { get; set; }
        public int? DepId { get; set; }
    }
}

