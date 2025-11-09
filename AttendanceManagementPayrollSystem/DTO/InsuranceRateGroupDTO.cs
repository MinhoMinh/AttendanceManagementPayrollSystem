namespace AttendanceManagementPayrollSystem.DTO
{
    public class InsuranceRateGroupDTO
    {
        public string Category { get; set; } = string.Empty;
        public List<InsuranceRateDTO> Inactive { get; set; } = new();
        public List<InsuranceRateDTO> Active { get; set; } = new();
        public List<InsuranceRateDTO> Upcoming { get; set; } = new();
    }
}
