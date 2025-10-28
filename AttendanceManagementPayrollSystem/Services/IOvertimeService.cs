using System.Collections.Generic;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.Interfaces
{
    public interface IOvertimeService
    {
        IEnumerable<OvertimeRequestDTO> GetOvertimeHistoryByEmployee(int empId);
        OvertimeRequest GetById(int id);
        void CreateOvertimeRequest(OvertimeRequest request);
        void ApproveOvertimeRequest(int reqId, int approverId);
        void RejectOvertimeRequest(int reqId, int approverId);
    }
}
