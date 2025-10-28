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
        public async Task<IEnumerable<LeaveRequest>> GetPendingAsync()
        {
            return await _context.LeaveRequests
                .Where(x => x.Status == "Pending")
                .ToListAsync();
        }
        public async Task<LeaveRequest?> GetByIdAsync(int id)
        {
            return await _context.LeaveRequests.FirstOrDefaultAsync(x => x.ReqId == id);
        }

        public async Task UpdateAsync(LeaveRequest request)
        {
            _context.LeaveRequests.Update(request);
            await _context.SaveChangesAsync();
        }
    }
}
