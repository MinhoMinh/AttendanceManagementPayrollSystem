using System.Collections.Generic;

namespace AttendanceManagementPayrollSystem.DTO
{
    public class DepartmentEmployeeGroupDTO
    {
        public int DepId { get; set; }
        public string DepName { get; set; }
        public List<EmployeeBasicDTO> Employees { get; set; } = new();
    }
}

