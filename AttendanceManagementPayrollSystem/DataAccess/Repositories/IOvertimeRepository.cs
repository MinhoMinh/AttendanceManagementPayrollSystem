using System.Collections.Generic;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface IOvertimeRepository
    {
        Task<IEnumerable<OvertimeRequest>> GetOvertimeByEmployeeId(int empId, DateOnly? startDate, DateOnly? endDate);
        OvertimeRequest GetById(int id);
        void Create(OvertimeRequest request);
        void Approve(int reqId, int approverId);
        void Reject(int reqId, int approverId);
        IEnumerable<OvertimeRate> GetRates();

        //Task<IGrouping<int, OvertimeRequest>> GetOvertimeByHeadId(int headId, DateOnly? start, DateOnly? end);
    }
}
