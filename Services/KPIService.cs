using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface KPIService
    {
        Task<KpiDto?> GetKpiAsync(int empId, int month, int year);
        //Task SaveEmployeeKpiAsync(int empId, string phase, EmployeeWithKpiDTO updatedKpi);
    }
}
