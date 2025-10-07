using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class PayrollRepositoryImpl : BaseRepositoryImpl, PayrollRepository
    {

        public PayrollRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context) 
        {
            
        }


        public async Task<PayrollRun> AddAsync(PayrollRun run)
        {
            _context.PayrollRuns.Add(run);
            await SaveChangesAsync();
            return run;
        }

        public async Task<PayrollRun?> FindAsync(int payrollId)
        {
            return await _context.PayrollRuns.FindAsync(payrollId);
        }

        public async Task<IEnumerable<PayrollRun>> GetAllAsync()
        {
            return await _context.PayrollRuns
                .Include(p => p.EmployeeSalaryPreviews)  // optional, if you need preview data
                .OrderByDescending(p => p.CreatedDate)
                .ToListAsync();
        }

        public async Task<SalaryPolicy?> GetActivePolicyAsync(int month, int year)
        {
            var periodStart = new DateTime(year, month, 1);
            return await _context.SalaryPolicies
                .Where(p => p.IsActive && p.EffectiveFrom <= periodStart)
                .OrderByDescending(p => p.EffectiveFrom)
                .FirstOrDefaultAsync();
        }

        public async Task Update(PayrollRun run)
        {
            _context.PayrollRuns.Update(run);
            await SaveChangesAsync();
        }
    }
}
