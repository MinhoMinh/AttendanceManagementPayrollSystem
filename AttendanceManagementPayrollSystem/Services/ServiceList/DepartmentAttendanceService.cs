using AttendanceManagementPayrollSystem.DTO;
using System.Collections.Generic;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface IAttendanceService
    {
        List<DepartmentAttendanceDTO> GetDepartmentAttendanceSummary(DateTime? startDate = null, DateTime? endDate = null);
        List<ClockinDTO> GetAllClockins(DateTime? startDate = null, DateTime? endDate = null);
    }
}

