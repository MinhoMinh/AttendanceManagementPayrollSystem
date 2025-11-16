using AttendanceManagementPayrollSystem.DTOs;

namespace AttendanceManagementPayrollSystem.DTO
{
    public class LeaveRequestGroupDTO
    {
        public string Department { get; set; } = string.Empty;

        public List<LeaveRequestDTO> pending { get; set; } = new();
        public List<LeaveRequestDTO> approved { get; set; } = new();
        public List<LeaveRequestDTO> denied { get; set; } = new();
    }
}
