using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface BonusService
    {
        Task<IEnumerable<BonusDTO>> GetAllAsync();
        Task AssignAsync(AssignBonusRequest request, int createdBy);
        Task<DepartmentBonusViewDTO> GetByDepartmentAsync(int depId);
    }

    public class BonusServiceImpl : BonusService
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public BonusServiceImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BonusDTO>> GetAllAsync()
        {
            var list = _context.Bonuses
                .OrderByDescending(b => b.BonusPeriod)
                .Select(b => new BonusDTO
                {
                    BonusId = b.BonusId,
                    BonusName = b.BonusName,
                    BonusAmount = b.BonusAmount,
                    BonusPeriod = b.BonusPeriod
                });

            return await Task.FromResult(list.ToList());
        }

        public async Task AssignAsync(AssignBonusRequest request, int createdBy)
        {
            var bonus = await _context.Bonuses.FindAsync(request.BonusId);
            if (bonus == null) throw new ArgumentException("Bonus not found");

            var bonusPeriod = new DateOnly(request.PeriodYear, request.PeriodMonth, 1);

            foreach (var empId in request.EmpIds.Distinct())
            {
                var rel = new EmpBonus
                {
                    EmpId = empId,
                    BonusId = request.BonusId,
                    DepId = request.DepId
                };
                _context.EmpBonuses.Add(rel);
            }

            // if overriding amount or period, create a new bonus entry scoped to that period
            if (request.OverrideAmount.HasValue || bonus.BonusPeriod != bonusPeriod)
            {
                int? createdByValue = null;
                try
                {
                    // Use createdBy only if such employee exists; otherwise leave null to avoid FK error
                    if (await _context.Employees.FindAsync(createdBy) != null)
                        createdByValue = createdBy;
                }
                catch
                {
                    createdByValue = null;
                }

                var customBonus = new Bonus
                {
                    BonusName = $"{bonus.BonusName} ({request.PeriodMonth}/{request.PeriodYear})",
                    BonusAmount = request.OverrideAmount ?? bonus.BonusAmount,
                    BonusPeriod = bonusPeriod,
                    CreatedAt = DateTime.Now,
                    CreatedBy = createdByValue
                };
                _context.Bonuses.Add(customBonus);
                await _context.SaveChangesAsync();

                // re-link EmpBonus to the custom bonus
                foreach (var empId in request.EmpIds.Distinct())
                {
                    var rel = new EmpBonus
                    {
                        EmpId = empId,
                        BonusId = customBonus.BonusId,
                        DepId = request.DepId
                    };
                    _context.EmpBonuses.Add(rel);
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DepartmentBonusViewDTO> GetByDepartmentAsync(int depId)
        {
            var dep = await _context.Departments.FindAsync(depId);
            if (dep == null)
            {
                return new DepartmentBonusViewDTO
                {
                    DepId = depId,
                    DepName = $"Dep #{depId}",
                    Employees = new List<EmployeeBonusViewDTO>()
                };
            }

            try
            {
                // join Emp_Bonus -> Bonus + Employee
                var rows = (from eb in _context.EmpBonuses.AsQueryable()
                            join e in _context.Employees on eb.EmpId equals e.EmpId
                            join b in _context.Bonuses on eb.BonusId equals b.BonusId
                            where (eb.DepId != null && eb.DepId == depId)
                                  || (eb.DepId == null && e.DepId == depId)
                            orderby e.EmpName
                            select new { e.EmpId, e.EmpName, Bonus = b }).ToList();

                var dict = new Dictionary<int, EmployeeBonusViewDTO>();
                foreach (var r in rows)
                {
                    if (!dict.TryGetValue(r.EmpId, out var dto))
                    {
                        dto = new EmployeeBonusViewDTO
                        {
                            EmpId = r.EmpId,
                            EmpName = r.EmpName
                        };
                        dict[r.EmpId] = dto;
                    }
                    dto.Bonuses.Add(new BonusDTO
                    {
                        BonusId = r.Bonus.BonusId,
                        BonusName = r.Bonus.BonusName,
                        BonusAmount = r.Bonus.BonusAmount,
                        BonusPeriod = r.Bonus.BonusPeriod
                    });
                }

                return new DepartmentBonusViewDTO
                {
                    DepId = dep.DepId,
                    DepName = dep.DepName,
                    Employees = dict.Values
                        .OrderBy(x => x.EmpName)
                        .ToList()
                };
            }
            catch
            {
                // In case table Emp_Bonus doesn't exist or any other SQL issue, return empty view for resilience
                return new DepartmentBonusViewDTO
                {
                    DepId = dep.DepId,
                    DepName = dep.DepName,
                    Employees = new List<EmployeeBonusViewDTO>()
                };
            }
        }
    }
}

