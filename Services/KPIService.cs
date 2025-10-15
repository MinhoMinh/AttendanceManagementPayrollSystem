using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface KPIService
    {
        Task<KpiDto?> GetKpiBySelfAsync(int empId, int month, int year);
        Task<KpiDto?> GetKpiByManagerAsync(int empId, int month, int year);
        Task<KpiDto?> GetKpiByHeadAsync(int empId, int month, int year);

        Task SaveKpiAsync(int empId, KpiDto kpiDto);
        Task EditKpiAsync(int empId, KpiDto kpiDto);
        Task AssignKpiAsync(int empId, KpiDto kpiDto);
        Task<List<EmployeeBasicDTO>> GetEmployeesWithKpiByManagerAsync(int month, int year);
        Task<List<EmployeeBasicDTO>> GetEmployeesWithKpiByHeadAsync(int headId, int month, int year);
    }
}
