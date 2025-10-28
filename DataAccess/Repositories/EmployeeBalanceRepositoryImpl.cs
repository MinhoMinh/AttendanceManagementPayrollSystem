using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class EmployeeBalanceRepository : IEmployeeBalanceRepository
    {
        private readonly AttendanceManagementPayrollSystemContext _context;

        public EmployeeBalanceRepository(AttendanceManagementPayrollSystemContext context)
        {
            _context = context;
        }

        public async Task<EmployeeBalance?> GetByEmployeeIdAsync(int empId)
        {
            return await _context.EmployeeBalances
                .Include(e => e.Emp)
                .FirstOrDefaultAsync(e => e.EmpId == empId);
        }
    }
}
