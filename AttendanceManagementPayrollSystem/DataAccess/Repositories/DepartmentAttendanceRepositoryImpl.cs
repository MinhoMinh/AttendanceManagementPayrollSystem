using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class AttendanceRepositoryImpl : IAttendanceRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public AttendanceRepositoryImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        public IEnumerable<Clockin> GetAllClockins(DateTime? startDate = null, DateTime? endDate = null)
        {
            var query = _context.Clockins
                .Include(c => c.Emp)
                .ThenInclude(e => e.Dep)
                .AsQueryable();

            if (startDate.HasValue)
                query = query.Where(c => c.Date >= startDate.Value);
            if (endDate.HasValue)
                query = query.Where(c => c.Date <= endDate.Value);

            return query.ToList();
        }
    }
}

