using System.Collections.Generic;

namespace AttendanceManagementPayrollSystem.DTO
{
    public class EmployeeBonusViewDTO
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public List<BonusDTO> Bonuses { get; set; } = new();
    }

    public class DepartmentBonusViewDTO
    {
        public int DepId { get; set; }
        public string DepName { get; set; }
        public List<EmployeeBonusViewDTO> Employees { get; set; } = new();
    }
}

