using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class LeaveRequestRepositoryImpl : ILeaveRequestRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public LeaveRequestRepositoryImpl(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        // ✅ Chỉ cần phương thức AddAsync để lưu dữ liệu
        public async Task<LeaveRequest> AddAsync(LeaveRequest request)
        {
            _context.LeaveRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}
