using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class DepartmentWeeklyShiftRepositoryImpl : BaseRepositoryImpl, DepartmentWeeklyShiftRepository
    {
        public DepartmentWeeklyShiftRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        public async Task<List<DepartmentWeeklyShiftViewDTO>> GetAllForViewAsync()
        {
            return await _context.DepartmentWeeklyShifts
                .Include(dws => dws.Dep)
                .Include(dws => dws.Shift)
                .Select(dws => new DepartmentWeeklyShiftViewDTO
                {
                    DeptShiftId = dws.DeptShiftId,
                    DepId = dws.DepId,
                    DepName = dws.Dep.DepName,   // từ navigation property
                    ShiftId = dws.ShiftId,
                    ShiftName = dws.Shift.ShiftName,
                    IsActive = dws.IsActive
                })
                .ToListAsync();
        }

        public async Task<DepartmentWeeklyShiftViewDTO> GetByIdForViewAsync(int deptShiftId)
        {
            var dws = await _context.DepartmentWeeklyShifts
                .Include(d => d.Dep)
                .Include(s => s.Shift)
                .FirstOrDefaultAsync(x => x.DeptShiftId == deptShiftId);

            if (dws == null) return null;

            return new DepartmentWeeklyShiftViewDTO
            {
                DeptShiftId = dws.DeptShiftId,
                DepId = dws.DepId,
                DepName = dws.Dep.DepName,
                ShiftId = dws.ShiftId,
                ShiftName = dws.Shift.ShiftName,
                IsActive = dws.IsActive
            };
        }

        public async Task<bool> UpdateAsync(DepartmentWeeklyShiftUpdateDTO dto)
        {
            var entity = await _context.DepartmentWeeklyShifts
                .FirstOrDefaultAsync(x => x.DeptShiftId == dto.DeptShiftId);

            if (entity == null) return false;

            entity.DepId = dto.DepId;
            entity.ShiftId = dto.ShiftId;
            entity.IsActive = dto.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
