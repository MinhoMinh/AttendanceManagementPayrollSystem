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

    }
}
