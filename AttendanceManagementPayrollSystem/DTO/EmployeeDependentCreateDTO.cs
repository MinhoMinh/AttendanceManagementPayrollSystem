namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeDependentCreateDTO
    {
        public int EmployeeId { get; set; }
        public string FullName { get; set; }
        public string Relationship { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string Gender { get; set; } // "M" hoặc "F"
        public string NationalId { get; set; }
        public bool IsTaxDependent { get; set; }
        public DateOnly EffectiveStartDate { get; set; }
        public DateOnly? EffectiveEndDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? Proof { get; set; } // URL hoặc null
    }
}
