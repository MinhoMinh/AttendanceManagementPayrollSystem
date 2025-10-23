using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class PayRunItemRepositoryImpl : BaseRepositoryImpl,PayRunItemRepository
    {
        public PayRunItemRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        public async Task<List<PayRunItemDTO>> GetPayRunItemsByEmpIdAsync(int empId)
        {
            var items = await _context.PayRunItems
                .Where(p => p.EmpId == empId)
                .Include(p => p.PayRunComponents)  // nạp bảng con
                .ToListAsync();

            // Ánh xạ sang DTO
            var result = items.Select(p => new PayRunItemDTO
            {
                PayRunItemId = p.PayRunItemId,
                PayRunId = p.PayRunId,
                EmpId = p.EmpId,
                GrossPay = p.GrossPay,
                Deductions = p.Deductions,
                NetPay = p.NetPay,
                Notes = p.Notes,
                Emp = p.Emp,
                PayRun = p.PayRun,
                PayRunComponents = p.PayRunComponents.Select(c => new PayRunComponentDTO
                {
                    PayRunComponentId = c.PayRunComponentId,
                    PayRunItemId = c.PayRunItemId,
                    ComponentType = c.ComponentType,
                    ComponentCode = c.ComponentCode,
                    Description = c.Description,
                    Amount = c.Amount,
                    Taxable = c.Taxable,
                    CreatedAt = c.CreatedAt,
                    Insurable = c.Insurable
                }).ToList()
            }).ToList();

            return result;
        }

    }
}
