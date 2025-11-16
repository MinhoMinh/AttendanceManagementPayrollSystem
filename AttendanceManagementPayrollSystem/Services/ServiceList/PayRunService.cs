using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.Helper;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface PayRunService
    {
        Task<PayRunDto> GenerateRegularPayRun(string name, int month, int year, int createdBy);

        Task SaveRegularPayRun(PayRunDto run);

        Task<IEnumerable<PayRunBasicDto>> GetAllAsync();

        Task<PayRunDto?> GetPayRunAsync(int id);

        Task<bool> ContainsValidPayRunInPeriod(int month, int year);

        Task<PayRunContext> GetPayRunContext(int month, int year);

        Task<bool> ApproveFirst(int approverId, int payRunId);

        Task<bool> ApproveFinal(int approverId, int payRunId);

        Task<bool> Reject(int approverId, int payRunId);

        Task<List<PayRunPreviewDTO>> GetPayRunByEmpIdAndDateAsync(int empId, int periodMonth, int periodYear);

        Task<List<PayRunPreviewDTO>?> GetPayRunsForEmployeeAsync(int empId, DateTime start, DateTime end);

    }
}
