namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeDependentDTO
    {
        public int DependentId { get; set; }
        public string FullName { get; set; }
        public string Relationship { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string NationalId { get; set; }
        public bool IsTaxDependent { get; set; }
        public DateOnly EffectiveStartDate { get; set; }
        public DateOnly? EffectiveEndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Proof { get; set; }
    }

    public class EmployeeWithDependentsDTO
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public List<EmployeeDependentDTO> Dependents { get; set; } = new();
    }
}
