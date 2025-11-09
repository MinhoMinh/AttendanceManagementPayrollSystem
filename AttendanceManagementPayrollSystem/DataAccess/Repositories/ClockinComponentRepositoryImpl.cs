using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class ClockinComponentRepositoryImpl : BaseRepositoryImpl, ClockinComponentRepository
    {
        public ClockinComponentRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        public async Task<ClockinComponent?> GetById(int id)
        {
            return await this._context.ClockinComponents.Where(c => c.Id == id).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(ClockinComponent clockinComponent)
        {
            this._context.ClockinComponents.Update(clockinComponent);
            await this._context.SaveChangesAsync();
        }
    }
}
