using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.DTOs;
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

        public async Task<IEnumerable<LeaveRequest>> GetAllLeaveRequestByDate(DateOnly? startDate, DateOnly? endDate)
        {
            return await this._context.LeaveRequests.Where(lr => lr.ReqDate >= startDate && lr.ReqDate <= endDate).ToListAsync();
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
            var list = await _context.LeaveRequests
            .Include(o => o.Type)
            .Include(o => o.ApprovedByNavigation)
            .Where(o => o.EmpId == empId && o.ReqDate >= startDate && o.ReqDate <= endDate)
            .OrderByDescending(o => o.ReqDate)
            .ToListAsync();

            //if (startDate.HasValue)
            //    query = query.Where(o => o.ReqDate >= startDate);

            //if (endDate.HasValue)
            //    query = query.Where(o => o.ReqDate <= endDate);

            //var list = await query
            //    .OrderByDescending(o => o.ReqDate)
            //    .ToListAsync();

            Console.WriteLine($"count {list.Count}");
            Console.WriteLine($"{list[0].ReqDate}");
            return list;
        }

        public IEnumerable<LeaveType> GetRates()
        {
            return _context.LeaveTypes;
        }

        public async Task<LeaveRequest> UpdateApprovalAsync(LeaveRequest entity)
        {
            _context.LeaveRequests.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<LeaveRequest> GetByIdAsync(int id)
        {
            return await _context.LeaveRequests.FirstOrDefaultAsync(x => x.ReqId == id);
        }

        public async Task<List<LeaveRequestGroupDTO>> GetGroupByDepIdAndDateRange(int depId, DateTime from, DateTime to)
        {
            var all = await _context.LeaveRequests
           .Include(l => l.Emp)
               .ThenInclude(e => e.Dep)
           .Include(l => l.ApprovedByNavigation)
           .Include(l=>l.Type)
           .Where(l => l.Emp != null
                       && l.Emp.Dep != null
                       && l.ReqDate >= DateOnly.FromDateTime(from)
                       && l.ReqDate <= DateOnly.FromDateTime(to)
                       && l.Emp.Dep.DepId==depId)
           .ToListAsync();

            var result = all
                .GroupBy(l => l.Emp.Dep.DepName)
                .Select(g => new LeaveRequestGroupDTO
                {
                    Department = g.Key,

                    pending = g.Where(x => x.Status.Equals("Pending"))
                               .Select(l => new LeaveRequestDTO
                               {
                                   EmpName=l.Emp.EmpName,
                                   ReqId = l.ReqId,
                                   EmpId = l.EmpId,
                                   TypeId = l.TypeId,
                                   ReqDate = l.ReqDate,
                                   StartDate = l.StartDate,
                                   EndDate = l.EndDate,
                                   NumbersOfDays = l.NumbersOfDays,
                                   Status = l.Status,
                                   Reason = l.Reason,
                                   ApprovedBy = l.ApprovedBy,
                                   ApprovedDate = l.ApprovedDate,
                                   ApprovedByName = l.ApprovedByNavigation?.EmpName ?? "Chưa được duyệt",
                                   TypeName=l.Type?.Name??"Không xác định"
                               })
                               .OrderByDescending(l => l.ReqDate)
                               .ToList(),

                    approved = g.Where(x => x.Status.Equals("Approved"))
                                .Select(l => new LeaveRequestDTO
                                {
                                    EmpName = l.Emp.EmpName,
                                    ReqId = l.ReqId,
                                    EmpId = l.EmpId,
                                    TypeId = l.TypeId,
                                    ReqDate = l.ReqDate,
                                    StartDate = l.StartDate,
                                    EndDate = l.EndDate,
                                    NumbersOfDays = l.NumbersOfDays,
                                    Status = l.Status,
                                    Reason = l.Reason,
                                    ApprovedBy = l.ApprovedBy,
                                    ApprovedDate = l.ApprovedDate,
                                    ApprovedByName = l.ApprovedByNavigation?.EmpName ?? "Chưa được duyệt",
                                    TypeName = l.Type?.Name ?? "Không xác định"
                                })
                                .OrderByDescending(l => l.ReqDate)
                                .ToList(),

                    denied = g.Where(x => x.Status.Equals("Denied"))
                               .Select(l => new LeaveRequestDTO
                               {
                                   EmpName = l.Emp.EmpName,
                                   ReqId = l.ReqId,
                                   EmpId = l.EmpId,
                                   TypeId = l.TypeId,
                                   ReqDate = l.ReqDate,
                                   StartDate = l.StartDate,
                                   EndDate = l.EndDate,
                                   NumbersOfDays = l.NumbersOfDays,
                                   Status = l.Status,
                                   Reason = l.Reason,
                                   ApprovedBy = l.ApprovedBy,
                                   ApprovedDate = l.ApprovedDate,
                                   ApprovedByName = l.ApprovedByNavigation?.EmpName ?? "Chưa được duyệt",
                                   TypeName = l.Type?.Name ?? "Không xác định"
                               })
                               .OrderByDescending(l => l.ReqDate)
                               .ToList()
                })
                .ToList();

            return result;
        }
    }
}
