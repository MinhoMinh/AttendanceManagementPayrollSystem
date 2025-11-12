using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface BonusService
    {
        Task<IEnumerable<BonusDTO>> GetAllAsync();
        Task AssignAsync(AssignBonusRequest request);
        Task<DepartmentBonusViewDTO> GetByDepartmentAsync(int depId);
        Task CreateAsync(BonusCreateRequest request);
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
            var list = await _context.Bonuses
                .OrderByDescending(b => b.BonusPeriod)
                .Select(b => new BonusDTO
                {
                    BonusId = b.BonusId,
                    BonusName = b.BonusName,
                    BonusAmount = b.BonusAmount,
                    BonusPeriod = b.BonusPeriod
                }).ToListAsync();

            return list;
        }

        public async Task CreateAsync(BonusCreateRequest request)
        {
            // Chuyển DateTime sang DateOnly (lấy ngày đầu tháng)
            var bonusPeriod = DateOnly.FromDateTime(request.BonusPeriod);

            // Đảm bảo luôn là ngày đầu tháng
            bonusPeriod = new DateOnly(bonusPeriod.Year, bonusPeriod.Month, 1);

            var bonus = new Bonus
            {
                BonusName = request.BonusName,
                BonusAmount = request.BonusAmount,
                BonusPeriod = bonusPeriod,
                CreatedAt = DateTime.Now
                // Đã xóa CreatedBy theo yêu cầu
            };

            _context.Bonuses.Add(bonus);
            await _context.SaveChangesAsync();
        }

        public async Task AssignAsync(AssignBonusRequest request)
        {
            var bonus = await _context.Bonuses.FindAsync(request.BonusId);
            if (bonus == null)
                throw new ArgumentException("Bonus not found");

            // Gán thưởng cho từng nhân viên trong request
            foreach (var empId in request.EmpIds.Distinct())
            {
                var exists = await _context.EmpBonuses.AnyAsync(eb =>
                    eb.EmpId == empId && eb.BonusId == request.BonusId && eb.DepId == request.DepId);

                if (!exists)
                {
                    _context.EmpBonuses.Add(new EmpBonus
                    {
                        EmpId = empId,
                        BonusId = request.BonusId,
                        DepId = request.DepId
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        public async Task<DepartmentBonusViewDTO> GetByDepartmentAsync(int depId)
        {
            // Load phòng ban và nhân viên
            var dep = await _context.Departments
                .Include(d => d.Employees)
                .FirstOrDefaultAsync(d => d.DepId == depId);

            if (dep == null)
                return new DepartmentBonusViewDTO
                {
                    DepId = depId,
                    DepName = $"Dep #{depId}",
                    Employees = new List<EmployeeBonusViewDTO>()
                };

            // Lấy tất cả nhân viên phòng ban, join với EmpBonus + Bonus (left join để lấy nhân viên chưa có bonus)
            var rows = await (from e in _context.Employees
                              where e.DepId == depId
                              join eb in _context.EmpBonuses on e.EmpId equals eb.EmpId into ebs
                              from eb in ebs.DefaultIfEmpty()
                              join b in _context.Bonuses on eb.BonusId equals b.BonusId into bs
                              from b in bs.DefaultIfEmpty()
                              orderby e.EmpName
                              select new { e.EmpId, e.EmpName, Bonus = b }).ToListAsync();

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

                if (r.Bonus != null)
                {
                    dto.Bonuses.Add(new BonusDTO
                    {
                        BonusId = r.Bonus.BonusId,
                        BonusName = r.Bonus.BonusName,
                        BonusAmount = r.Bonus.BonusAmount,
                        BonusPeriod = r.Bonus.BonusPeriod
                    });
                }
            }

            return new DepartmentBonusViewDTO
            {
                DepId = dep.DepId,
                DepName = dep.DepName,
                Employees = dict.Values.OrderBy(x => x.EmpName).ToList()
            };
        }
    }
}