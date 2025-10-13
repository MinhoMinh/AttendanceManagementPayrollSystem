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

        public async Task<KpiDto?> GetKpiBySelfAsync(int empId, int month, int year)
        {
            var kpi = await _context.Kpis
        .Include(k => k.Kpicomponents)
        .Where(k => k.EmpId == empId && k.PeriodMonth == month && k.PeriodYear == year)
        .Select(k => new KpiDto
        {
            KpiId = k.KpiId,
            PeriodMonth = k.PeriodMonth,
            PeriodYear = k.PeriodYear,
            KpiRate = k.KpiRate,
            //KpiMode = KPIAccessHelper.GetKpiMode(k.PeriodMonth, k.PeriodYear, "employee"),
            KpiMode = "self",
            Components = k.Kpicomponents.Select(c => new KpiComponentDto
            {
                KpiComponentId = c.KpiCompId,
                Name = c.Name,
                Description = c.Description,
                TargetValue = c.TargetValue,
                AchievedValue = c.AchievedValue,
                Weight = c.Weight,
                SelfScore = c.SelfScore,
                AssignedScore = c.AssignedScore
            }).ToList()
        })
        .FirstOrDefaultAsync();

            return kpi;
        }

        public async Task SaveKpiAsync(int empId, KpiDto kpiDto)
        {
            var kpi = await _context.Kpis
                .Include(k => k.Kpicomponents)
                .FirstOrDefaultAsync(k =>
                    k.EmpId == empId &&
                    k.PeriodMonth == kpiDto.PeriodMonth &&
                    k.PeriodYear == kpiDto.PeriodYear);

            if (kpi == null)
                throw new KeyNotFoundException("KPI not found for this employee and period.");

            foreach (var compDto in kpiDto.Components)
            {
                var comp = kpi.Kpicomponents.FirstOrDefault(c => c.KpiCompId == compDto.KpiComponentId);
                if (comp != null)
                {
                    comp.AchievedValue = compDto.AchievedValue;
                    comp.SelfScore = compDto.SelfScore;
                    comp.AchievedValue = compDto.AchievedValue;
                }
            }

            await _context.SaveChangesAsync();
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
