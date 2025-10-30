using System.Collections.Generic;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface IOvertimeRepository
    {
        IEnumerable<OvertimeRequest> GetOvertimeByEmployeeId(int empId);
        OvertimeRequest GetById(int id);
        void Create(OvertimeRequest request);
        void Approve(int reqId, int approverId);
        void Reject(int reqId, int approverId);
    }
}
