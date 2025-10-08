namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeWithKpiDTO
    {
        public int EmpId { get; set; }
        public string Name { get; set; }
        public List<KpiDto> Kpis { get; set; } = new();
    }

    public class KpiDto
    {
        public int KpiId { get; set; }
        public int PeriodYear { get; set; }
        public int PeriodMonth { get; set; }
        public int KpiRate { get; set; }
        public List<KpiComponentDto> Components { get; set; } = new();
    }

    public class KpiComponentDto
    {
        public int KpiComponentId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TargetValue { get; set; }
        public int AchievedValue { get; set; }
        public int Weight { get; set; }
        public decimal SelfScore { get; set; }
        public decimal AssignedScore { get; set; }
    }
}
