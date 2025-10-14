using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface KPIService
    {
        Task<KpiDto?> GetKpiBySelfAsync(int empId, int month, int year);
        //Task SaveEmployeeKpiAsync(int empId, string phase, EmployeeWithKpiDTO updatedKpi);
        Task SaveKpiAsync(int empId, KpiDto kpiDto);
    }
}
