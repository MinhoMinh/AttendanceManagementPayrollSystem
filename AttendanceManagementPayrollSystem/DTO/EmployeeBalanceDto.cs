namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeBalanceDto
    {
        public int Id { get; set; }

        public int EmpId { get; set; }

        public decimal PtoAvailable { get; set; }

        public decimal PtoUsed { get; set; }

        public decimal PtoTotal { get; set; }

        public decimal SickAvailable { get; set; }

        public decimal SickUsed { get; set; }

        public decimal SickTotal { get; set; }

        public decimal VacationAvailable { get; set; }

        public decimal VacationUsed { get; set; }

        public decimal VacationTotal { get; set; }

        public decimal OvertimeAvailable { get; set; }

        public decimal OvertimeUsed { get; set; }

        public decimal OvertimeTotal { get; set; }

        public DateTime? LastUpdated { get; set; }
    }


}
