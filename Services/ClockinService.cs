using System;
using System.Data;
using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface ClockinService
    {
        //public List<DailyDetailDTO> ParseClockData(string clockLog, string breakdown);
        //public Task<ClockinDTO?> GetClockinsByEmployeeAsync(int empId, int month, int year);

        Task<List<ClockinDTO>?> ReadClockinData(DataTable table);
        Task SaveClockinData(List<ClockinDTO> dtos);
    }
}