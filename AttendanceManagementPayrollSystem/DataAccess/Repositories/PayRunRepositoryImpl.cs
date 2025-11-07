//using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.EntityFrameworkCore;
using System;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class PayRunRepositoryImpl : BaseRepositoryImpl, PayRunRepository
    {

        public PayRunRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context) 
        {
            
        }

        public async Task<PayRun> AddAsync(PayRun run)
        {
            _context.PayRuns.Add(run);
            await SaveChangesAsync();
            return run;
        }

        public async Task<PayRun?> FindAsync(int payrollId)
        {
            return await _context.PayRuns.FindAsync(payrollId);
        }

        public async Task<IEnumerable<PayRun>> GetAllAsync()
        {
            return await _context.PayRuns
                .Include(p => p.PayRunItems)
                    .ThenInclude(item => item.PayRunComponents)// optional, if you need preview data
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<PayRunDto?> GetDtoAsync(int id)
        {
            var entity = await _context.PayRuns
                .Include(p => p.PayRunItems)
                    .ThenInclude(i => i.PayRunComponents)
                .FirstOrDefaultAsync(p => p.PayrollRunId == id);

            if (entity == null)
                return null;

            var employeeIds = new List<int>();
            if (entity.CreatedBy.HasValue) employeeIds.Add(entity.CreatedBy.Value);
            if (entity.ApprovedFirstBy.HasValue) employeeIds.Add(entity.ApprovedFirstBy.Value);
            if (entity.ApprovedFinalBy.HasValue) employeeIds.Add(entity.ApprovedFinalBy.Value);
            if (entity.RejectedBy.HasValue) employeeIds.Add(entity.RejectedBy.Value);
            foreach (var emp in entity.PayRunItems) 
                employeeIds.Add(emp.EmpId);


            var names = await _context.Employees
                .Where(e => employeeIds.Contains(e.EmpId))
                .Select(e => new { e.EmpId, e.EmpName })
                .ToListAsync();

            string GetName(int? id) =>
                id.HasValue ? names.FirstOrDefault(x => x.EmpId == id.Value)?.EmpName : null;

            return new PayRunDto
            {
                PayrollRunId = entity.PayrollRunId,
                Name = entity.Name,
                PeriodMonth = entity.PeriodMonth,
                PeriodYear = entity.PeriodYear,
                Status = entity.Status,
                CreatedDate = entity.CreatedDate,
                Type = entity.Type,
                CreatedByName = GetName(entity.CreatedBy),
                ApprovedFirstByName = GetName(entity.ApprovedFirstBy),
                ApprovedFirstAt = entity.ApprovedFirstAt,
                ApprovedFinalByName = GetName(entity.ApprovedFinalBy),
                ApprovedFinalAt = entity.ApprovedFinalAt,
                RejectedByName = GetName(entity.RejectedBy),
                RejectedAt = entity.RejectedAt,
                PayRunItems = entity.PayRunItems.Select(i => new PayRunItemDto
                {
                    ItemId = i.PayRunItemId,
                    EmpId = i.EmpId,
                    EmpName = GetName(i.EmpId),
                    GrossPay = i.GrossPay,
                    Deductions = i.Deductions,
                    NetPay = i.NetPay,
                    Notes = i.Notes,
                    Components = i.PayRunComponents.Select(c => new PayRunComponentDto
                    {
                        ComponentId = c.PayRunComponentId,
                        ComponentType = c.ComponentType,
                        ComponentCode = c.ComponentCode,
                        Description = c.Description,
                        Amount = c.Amount,
                        Taxable = c.Taxable,
                        CreatedAt = c.CreatedAt
                    }).ToList()
                }).ToList()
            };
        }

        public async Task<SalaryPolicy?> GetActivePolicyAsync(int month, int year)
        {
            var periodStart = new DateTime(year, month, 1);
            return await _context.SalaryPolicies
                .Where(p => p.IsActive && p.EffectiveFrom <= periodStart)
                .OrderByDescending(p => p.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task Update(PayRun run)
        {
            _context.PayRuns.Update(run);
            await SaveChangesAsync();
        }

        public async Task<List<Employee>> GetEmployeesWithComponents(int month, int year)
        {
            var results = await _context.Employees
                .Where(e => e.Clockins.Any(p => p.Date.Month == month && p.Date.Year == year)
                    || e.KpiEmps.Any(k => k.PeriodMonth == month && k.PeriodYear == year))
                .Include(e => e.Clockins
                    .Where(p => p.Date.Month == month && p.Date.Year == year))
                .Include(e => e.KpiEmps
                    .Where(k => k.PeriodMonth == month && k.PeriodYear == year))
                    .ThenInclude(k => k.Kpicomponents)
                .ToListAsync();

            return results;
        }

        public async Task<bool> ContainsValidPayRunInPeriod(int month, int year)
        {
            return await _context.PayRuns.AnyAsync(p => p.PeriodMonth == month && p.PeriodYear == year && p.Status != "Void");
        }

        public async Task SaveRegularPayRun(PayRun run)
        {
            await _context.PayRuns.AddAsync(run);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PayRunPreviewDTO>> GetPayRunByEmpIdAndDateAsync(int empId, int periodMonth, int periodYear)
        {
            var items = await _context.PayRuns
                .Where(p => p.PeriodMonth >= periodMonth
                        && p.PeriodYear >= periodYear
                        && p.Status.Equals("FinalApproved")
                        && p.PayRunItems.Any(i => i.EmpId == empId))
                .Select(p => new PayRunPreviewDTO
                {
                    PayrollRunId = p.PayrollRunId,
                    Name = p.Name,
                    PeriodMonth = p.PeriodMonth,
                    PeriodYear = p.PeriodYear,
                    CreatedDate = p.CreatedDate,
                    Type = p.Type,
                    PayRunItems = p.PayRunItems
                        .Where(i => i.EmpId == empId)
                        .Select(i => new PayRunItemDto
                        {
                            ItemId = i.PayRunItemId,
                            EmpId = i.EmpId,
                            EmpName = i.Emp != null ? i.Emp.EmpName : null,
                            GrossPay = i.GrossPay,
                            Deductions = i.Deductions,
                            NetPay = i.NetPay,
                            Notes = i.Notes,
                            Components = i.PayRunComponents
                                    .Select(c => new PayRunComponentDto
                                    {
                                        ComponentId = c.PayRunComponentId,
                                        ComponentType = c.ComponentType,
                                        ComponentCode = c.ComponentCode,
                                        Description = c.Description,
                                        Amount = c.Amount,
                                        Taxable = c.Taxable,
                                        Insurable = c.Insurable,
                                        CreatedAt = c.CreatedAt
                                    }).ToList()
                        }).ToList()
                }).ToListAsync();


            return items;

        }
    }
}
