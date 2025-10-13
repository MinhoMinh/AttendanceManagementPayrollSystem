using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface PayrollService
    {
        Task<PayrollRunDTO>  GeneratePayrollAsync(string name, int periodMonth, int periodYear, int createdBy);

        Task<PayrollRunDTO> ApproveFirstAsync(int id, int approvedBy);
        Task<PayrollRunDTO> ApproveFinalAsync(int id, int approvedBy);
        Task<PayrollRunDTO> RejectAsync(int id, int rejectedBy);
        Task<IEnumerable<PayrollRunDTO>> GetAllAsync();
    }
}
