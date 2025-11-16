using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class LeaveRequestRepositoryImpl : BaseRepositoryImpl, ILeaveRequestRepository
    {

        public LeaveRequestRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context) { }

        public async Task<LeaveRequest> AddAsync(LeaveRequest request)
        {
            _context.LeaveRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }
        public async Task<IEnumerable<LeaveRequest>> GetByEmployeeIdAsync(int empId)
        {
            return await _context.LeaveRequests
                .Where(lr => lr.EmpId == empId)
                .OrderByDescending(lr => lr.ReqDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<LeaveRequest>> GetLeaveByEmployeeId(int empId, DateOnly? startDate, DateOnly? endDate)
        {
            var query = _context.LeaveRequests
            .Include(o => o.Type)
            .Include(o => o.ApprovedByNavigation)
            .Where(o => o.EmpId == empId);

            if (startDate.HasValue)
                query = query.Where(o => o.ReqDate >= startDate);

            if (endDate.HasValue)
                query = query.Where(o => o.ReqDate <= endDate);

            var list = await query
                .OrderByDescending(o => o.ReqDate)
                .ToListAsync();

            return list;
        }

        public IEnumerable<LeaveType> GetRates()
        {
                return _context.LeaveTypes;
        }
    }
}
