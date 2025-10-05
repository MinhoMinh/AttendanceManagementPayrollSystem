using AttendanceManagementPayrollSystem.Models;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class ClockinRepositoryImpl : BaseRepositoryImpl, ClockinRepository
    {

        public ClockinRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Clockin>> GetByDateRangeAsync(int periodMonth, int periodYear)
        {
            var startDate = new DateTime(periodYear, periodMonth, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await _context.Clockins
                .Where(c => c.Date >= startDate && c.Date <= endDate)
                .ToListAsync();
        }
    }
}
