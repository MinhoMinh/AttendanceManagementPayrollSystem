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
            return await GetKpiAsync(empId, month, year, "employee");
        }

        public async Task<KpiDto?> GetKpiByManagerAsync(int empId, int month, int year)
        {
            return await GetKpiAsync(empId, month, year, "manager");
        }

        public async Task<KpiDto?> GetKpiByHeadAsync(int empId, int month, int year)
        {
            return await GetKpiAsync(empId, month, year, "head");
        }

        private async Task<KpiDto?> GetKpiAsync(int empId, int month, int year, string role)
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
                AssignedBy = k.AssignedBy,
                KpiMode = KPIAccessHelper.GetKpiMode(k.PeriodMonth, k.PeriodYear, role),
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

            if (kpi == null && role == "head")
            {
                var mode = KPIAccessHelper.GetKpiMode(month, year, role);
                if (mode == "edit")
                {
                    kpi = new KpiDto
                    {
                        KpiId = 0, // indicate it is new
                        PeriodMonth = month,
                        PeriodYear = year,
                        KpiRate = 100,
                        KpiMode = mode,
                        AssignedBy = 0,
                        Components = new List<KpiComponentDto>()
                    };
                }
            }

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
                    comp.SelfScore = compDto.SelfScore;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task EditKpiAsync(int empId, KpiDto kpiDto)
        {
            var kpi = await _context.Kpis
                .Include(k => k.Kpicomponents)
                .FirstOrDefaultAsync(k =>
                    k.EmpId == empId &&
                    k.PeriodMonth == kpiDto.PeriodMonth &&
                    k.PeriodYear == kpiDto.PeriodYear);

            if (kpi == null)
            {
                kpi = new Kpi
                {
                    EmpId = empId,
                    PeriodMonth = kpiDto.PeriodMonth,
                    PeriodYear = kpiDto.PeriodYear,
                    KpiRate = kpiDto.KpiRate,
                    //AssignedBy = kpiDto.AssignedBy,
                    Kpicomponents = kpiDto.Components.Select(c => new Kpicomponent
                    {
                        Name = c.Name,
                        Description = c.Description,
                        TargetValue = c.TargetValue,
                        AchievedValue = c.AchievedValue,
                        Weight = c.Weight,
                        SelfScore = c.SelfScore,
                        AssignedScore = c.AssignedScore
                    }).ToList()
                };

                _context.Kpis.Add(kpi);
            }
            else
            {
                // Update KPI fields
                kpi.KpiRate = kpiDto.KpiRate;
                //kpi.AssignedBy = kpiDto.AssignedBy;

                // Update existing components
                foreach (var compDto in kpiDto.Components)
                {
                    var existingComp = kpi.Kpicomponents.FirstOrDefault(c => c.KpiCompId == compDto.KpiComponentId);
                    if (existingComp != null)
                    {
                        // Update existing
                        existingComp.Name = compDto.Name;
                        existingComp.Description = compDto.Description;
                        existingComp.TargetValue = compDto.TargetValue;
                        existingComp.AchievedValue = compDto.AchievedValue;
                        existingComp.Weight = compDto.Weight;
                        existingComp.SelfScore = compDto.SelfScore;
                        existingComp.AssignedScore = compDto.AssignedScore;
                    }
                    else
                    {
                        // Add new component
                        kpi.Kpicomponents.Add(new Kpicomponent
                        {
                            Name = compDto.Name,
                            Description = compDto.Description,
                            TargetValue = compDto.TargetValue,
                            AchievedValue = compDto.AchievedValue,
                            Weight = compDto.Weight,
                            SelfScore = compDto.SelfScore,
                            AssignedScore = compDto.AssignedScore
                        });
                    }
                }

                // Remove deleted components
                var dtoIds = kpiDto.Components.Select(c => c.KpiComponentId).ToHashSet();
                var toRemove = kpi.Kpicomponents.Where(c => !dtoIds.Contains(c.KpiCompId)).ToList();
                _context.Kpicomponents.RemoveRange(toRemove);
            }

            await _context.SaveChangesAsync();
        }

        public async Task AssignKpiAsync(int empId, KpiDto kpiDto)
        {
            var kpi = await _context.Kpis
                .Include(k => k.Kpicomponents)
                .FirstOrDefaultAsync(k =>
                    k.EmpId == empId &&
                    k.PeriodMonth == kpiDto.PeriodMonth &&
                    k.PeriodYear == kpiDto.PeriodYear);

            if (kpi == null)
                throw new KeyNotFoundException("KPI not found for this employee and period.");

            kpi.AssignedBy = kpiDto.AssignedBy;

            foreach (var compDto in kpiDto.Components)
            {
                var comp = kpi.Kpicomponents.FirstOrDefault(c => c.KpiCompId == compDto.KpiComponentId);
                if (comp != null)
                {
                    comp.AchievedValue = compDto.AchievedValue;
                    comp.AssignedScore = compDto.AssignedScore;
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<EmployeeBasicDTO>> GetEmployeesWithKpiByManagerAsync(int month, int year)
        {
            var employees = await _context.Employees
                .Where(emp => emp.KpiEmps.Any(k => k.PeriodMonth == month && k.PeriodYear == year))
                .Select(emp => new EmployeeBasicDTO
                {
                    EmpId = emp.EmpId,
                    EmpName = emp.EmpName,
                })
                .ToListAsync();

            return employees;
        }

        public async Task<List<EmployeeBasicDTO>> GetEmployeesWithKpiByHeadAsync(int headId, int month, int year)
        {
            var mode = KPIAccessHelper.GetKpiMode(month, year, "head");

            var headDepId = await _context.Employees
                .Where(h => h.EmpId == headId)
                .Select(h => h.DepId) // assuming Employee has DepId
                .FirstOrDefaultAsync();

            if (headDepId == 0)
                return new List<EmployeeBasicDTO>();

            List<EmployeeBasicDTO>? employees = null;

            if (mode == "edit")
            {
                //find all emp in the same dep with headId
                employees = await _context.Employees
                    .Where(emp => emp.DepId == headDepId)
                    .Select(emp => new EmployeeBasicDTO
                    {
                        EmpId = emp.EmpId,
                        EmpName = emp.EmpName,
                    })
                    .ToListAsync();
            }
            else
            {
                //find exisitng only for  emp in the same dep with headId
                employees = await _context.Employees
                    .Where(emp => emp.DepId == headDepId &&
                                  emp.KpiEmps.Any(k => k.PeriodMonth == month && k.PeriodYear == year))
                    .Select(emp => new EmployeeBasicDTO
                    {
                        EmpId = emp.EmpId,
                        EmpName = emp.EmpName,
                    })
                    .ToListAsync();
            }

            return employees;
        }
    }
}
