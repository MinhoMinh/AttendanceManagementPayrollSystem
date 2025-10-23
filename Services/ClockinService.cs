using System;
using System.Data;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface ClockinService
    {
        //public List<DailyDetailDTO> ParseClockData(string clockLog, string breakdown);
        Task<IEnumerable<ClockinDTO>> GetByEmployeeAsync(int empId, DateTime startDate, int months);
        public Task<ClockinDTO?> GetClockinsByEmployeeAsync(int empId, int month, int year);

        Task<List<ClockinDTO>?> ReadClockinData(DataTable table);
        Task SaveClockinData(List<ClockinDTO> dtos);
    }
}