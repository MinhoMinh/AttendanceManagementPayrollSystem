using AttendanceManagementPayrollSystem.Models;
using System.Collections.Generic;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface IAttendanceRepository  
    {
        IEnumerable<Clockin> GetAllClockins(DateTime? startDate = null, DateTime? endDate = null);
    }
}
