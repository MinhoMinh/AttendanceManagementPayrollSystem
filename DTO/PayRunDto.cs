namespace AttendanceManagementPayrollSystem.DTO
{
    public class PayRunDto
    {
        public int PayrollRunId { get; set; }
        public string Name { get; set; }
        public int PeriodMonth { get; set; }
        public int PeriodYear { get; set; }
        public string Status { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ApprovedFirstBy { get; set; }
        public string? ApprovedFirstByName { get; set; }
        public DateTime? ApprovedFirstAt { get; set; }
        public int? ApprovedFinalBy { get; set; }
        public string? ApprovedFinalByName { get; set; }
        public DateTime? ApprovedFinalAt { get; set; }
        public string Type { get; set; }

        public List<PayRunItemDto> PayRunItems { get; set; } = new();
    }

    public class PayRunItemDto
    {
        public int ItemId { get; set; }
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public decimal GrossPay { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
        public string Notes { get; set; }
        public List<PayRunComponentDto> Components { get; set; } = new();
    }

    public class PayRunComponentDto
    {
        public int ComponentId { get; set; }
        public string ComponentType { get; set; }
        public string ComponentCode { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public bool? Taxable { get; set; }
        public DateTime? CreatedAt { get; set; }
    }

    public class PayRunBasicDto
    {

        public int PayrollRunId { get; set; }

        public string Name { get; set; }

        public int PeriodMonth { get; set; }

        public int PeriodYear { get; set; }

        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
