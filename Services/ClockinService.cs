using System;
using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface ClockinService
    {
        public List<DailyDetailDTO> ParseClockData(string clockLog, string breakdown);
        public Task<List<ClockinDTO>> GetClockinsByEmployeeAsync(int empId, int month, int year);
    }
}