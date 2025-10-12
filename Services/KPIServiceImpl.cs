using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.Services
{
    public class KPIServiceImpl : KPIService
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public KPIServiceImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        public async Task<KpiDto?> GetKpiAsync(int empId, int month, int year)
        {
            var employee = await _context.Employees
            .Where(e => e.EmpId == empId)
            .Include(e => e.KpiEmps)
                .ThenInclude(k => k.Kpicomponents)
            .FirstOrDefaultAsync();

            if (employee == null)
                return null;

            var kpi = employee.KpiEmps
                .FirstOrDefault(k => k.PeriodYear == year && k.PeriodMonth == month);

#pragma warning disable CS8601 // Possible null reference assignment.
            var dto = new KpiDto
            {
                KpiId = kpi.KpiId,
                PeriodYear = kpi.PeriodYear,
                PeriodMonth = kpi.PeriodMonth,
                KpiRate = kpi.KpiRate,
                KpiMode = KPIAccessHelper.GetKpiMode(kpi.PeriodYear, kpi.PeriodMonth, "employee"),
                Components = kpi.Kpicomponents.Select(c => new KpiComponentDto
                {
                    KpiComponentId = c.KpiCompId,
                    Name = c.Name,
                    Description = c.Description,
                    TargetValue = c.TargetValue,
                    AchievedValue = c.AchievedValue,   // now nullable
                    Weight = c.Weight,
                    SelfScore = c.SelfScore,           // now nullable
                    AssignedScore = c.AssignedScore    // now nullable
                }).ToList()

            };
#pragma warning restore CS8601 // Possible null reference assignment.

            return dto;

        }

        //public async Task SaveEmployeeKpiAsync(int empId, string phase, EmployeeWithKpiDTO updatedKpi)
        //{
        //    var kpiEntity = await _context.Kpis
        //        .Include(k => k.Kpicomponents)
        //        .FirstOrDefaultAsync(k => k.EmpId == empId && k.KpiId == updatedKpi.Kpi.KpiId);

        //    if (kpiEntity == null)
        //        throw new Exception("KPI not found.");

        //    foreach (var compDto in updatedKpi.Kpi.Components)
        //    {
        //        Kpicomponent compEntity;

        //        if (compDto.KpiComponentId > 0) // existing component
        //        {
        //            compEntity = kpiEntity.Kpicomponents.FirstOrDefault(c => c.KpiCompId == compDto.KpiComponentId);
        //            if (compEntity == null) continue;
        //        }
        //        else // new component added in Assign phase
        //        {
        //            if (phase != "Assign") continue; // only allow adding in Assign
        //            compEntity = new Kpicomponent();
        //            kpiEntity.Kpicomponents.Add(compEntity);
        //        }

        //        // Update fields based on phase
        //        switch (phase)
        //        {
        //            case "Assign":
        //                compEntity.Name = compDto.Name;
        //                compEntity.Description = compDto.Description;
        //                compEntity.TargetValue = compDto.TargetValue;
        //                compEntity.Weight = compDto.Weight;
        //                break;
        //            case "SelfScore":
        //                compEntity.AchievedValue = compDto.AchievedValue;
        //                compEntity.SelfScore = compDto.SelfScore;
        //                break;
        //            case "Finalize":
        //                compEntity.AssignedScore = compDto.AssignedScore;
        //                break;
        //            case "ViewOnly":
        //                // do nothing
        //                break;
        //        }
        //    }

        //    await _context.SaveChangesAsync();
        //}

    }
}
