using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface PayRunItemService
    {
        Task<List<PayRunItemDTO>> GetPayRunItemsByEmpIdAsync(int empId);
        

    }
}
