using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class ClockInAdjustmentRequestRepositoryImpl : BaseRepositoryImpl, ClockInAdjustmentRequestRepository
    {
        public ClockInAdjustmentRequestRepositoryImpl(AttendanceManagementPayrollSystemContext context) : base(context)
        {
        }

        public async Task AddAsync(ClockInAdjustmentRequest clockInAdjustmentRequest)
        {
            this._context.AddAsync(clockInAdjustmentRequest);
            await this._context.SaveChangesAsync();
        }

        public async Task<ClockInAdjustmentRequest?> GetAdjustmentRequestByIdAsync(int id)
        {
            return await this._context.ClockInAdjustmentRequests.Where(c => c.RequestId == id).FirstOrDefaultAsync();
        }

        public Task<List<ClockinAdjustmentRequestDTO>> GetByEmpID(int id)
        {
            var result = this._context.ClockInAdjustmentRequests.Where(c => c.EmployeeId == id)
                                                              .Select(c => new ClockinAdjustmentRequestDTO
                                                              {
                                                                  RequestId = c.RequestId,
                                                                  EmployeeId = c.EmployeeId,
                                                                  ClockInComponentId = c.ClockInComponentId,
                                                                  RequestedValue = c.RequestedValue,
                                                                  Message = c.Message,
                                                                  Attachment = c.Attachment,
                                                                  Status = c.Status,
                                                                  ApproverId = c.ApproverId,
                                                                  Comment = c.Comment,
                                                                  CreatedAt = c.CreatedAt,
                                                                  EmployeeName = c.Employee.EmpName,
                                                                  ApproverName = c.Approver.EmpName
                                                              }).OrderByDescending(c => c.CreatedAt).ToListAsync();
            return result;
        }

        public async Task<List<ClockinAdjustmentRequestGroupDTO>> GetGroupByDepID()
        {
            var all = await this._context.ClockInAdjustmentRequests
                                    .Include(c => c.Employee)
                                        .ThenInclude(e=>e.Dep)
                                    .Include(c => c.Approver)
                                    .Include(c => c.ClockInComponent)
                                    .Where(c=>c.Employee!=null && c.Employee.Dep!=null)
                                    .ToListAsync();

            var result = all.GroupBy(c => c.Employee.Dep.DepName)
                           .Select(g => new ClockinAdjustmentRequestGroupDTO
                           {
                               Department = g.Key,
                               pending = g.Where(g => g.Status.Equals("Pending"))
                                        .Select(c => new ClockinAdjustmentRequestDTO
                                        {
                                            RequestId = c.RequestId,
                                            EmployeeId = c.EmployeeId,
                                            ClockInComponentId = c.ClockInComponentId,
                                            RequestedValue = c.RequestedValue,
                                            Message = c.Message,
                                            Attachment = c.Attachment,
                                            Status = c.Status,
                                            ApproverId = c.ApproverId,
                                            Comment = c.Comment,
                                            CreatedAt = c.CreatedAt,
                                            EmployeeName = c.Employee?.EmpName ?? "Không xác định",
                                            ApproverName = c.Approver?.EmpName ?? "Chưa được duyệt"
                                        }).OrderByDescending(c => c.CreatedAt).ToList(),
                               approved = g.Where(g => g.Status.Equals("Approved"))
                                         .Select(c => new ClockinAdjustmentRequestDTO
                                         {
                                             RequestId = c.RequestId,
                                             EmployeeId = c.EmployeeId,
                                             ClockInComponentId = c.ClockInComponentId,
                                             RequestedValue = c.RequestedValue,
                                             Message = c.Message,
                                             Attachment = c.Attachment,
                                             Status = c.Status,
                                             ApproverId = c.ApproverId,
                                             Comment = c.Comment,
                                             CreatedAt = c.CreatedAt,
                                             EmployeeName = c.Employee?.EmpName ?? "Không xác định",
                                             ApproverName = c.Approver?.EmpName ?? "Chưa được duyệt"
                                         }).OrderByDescending(c => c.CreatedAt).ToList(),
                               denied = g.Where(g => g.Status.Equals("Denied"))
                                         .Select(c => new ClockinAdjustmentRequestDTO
                                         {
                                             RequestId = c.RequestId,
                                             EmployeeId = c.EmployeeId,
                                             ClockInComponentId = c.ClockInComponentId,
                                             RequestedValue = c.RequestedValue,
                                             Message = c.Message,
                                             Attachment = c.Attachment,
                                             Status = c.Status,
                                             ApproverId = c.ApproverId,
                                             Comment = c.Comment,
                                             CreatedAt = c.CreatedAt,
                                             EmployeeName = c.Employee?.EmpName ?? "Không xác định",
                                             ApproverName = c.Approver?.EmpName ?? "Chưa được duyệt"
                                         }).OrderByDescending(c => c.CreatedAt).ToList()

                           }).ToList();
            return result;


        }

        public async Task<List<ClockinAdjustmentRequestGroupDTO>> GetGroupByDepIdAndDateRange(DateTime from, DateTime to)
        {
            var all = await this._context.ClockInAdjustmentRequests
                                    .Include(c => c.Employee)
                                        .ThenInclude(e => e.Dep)
                                    .Include(c => c.Approver)
                                    .Include(c => c.ClockInComponent)
                                    .Where(c => c.Employee != null && c.Employee.Dep != null && c.CreatedAt.Value.Date>=from.Date && c.CreatedAt.Value.Date <=to.Date)
                                    .ToListAsync();

            var result = all.GroupBy(c => c.Employee.Dep.DepName)
                           .Select(g => new ClockinAdjustmentRequestGroupDTO
                           {
                               Department = g.Key,
                               pending = g.Where(g => g.Status.Equals("Pending"))
                                        .Select(c => new ClockinAdjustmentRequestDTO
                                        {
                                            RequestId = c.RequestId,
                                            EmployeeId = c.EmployeeId,
                                            ClockInComponentId = c.ClockInComponentId,
                                            RequestedValue = c.RequestedValue,
                                            Message = c.Message,
                                            Attachment = c.Attachment,
                                            Status = c.Status,
                                            ApproverId = c.ApproverId,
                                            Comment = c.Comment,
                                            CreatedAt = c.CreatedAt,
                                            EmployeeName = c.Employee?.EmpName ?? "Không xác định",
                                            ApproverName = c.Approver?.EmpName ?? "Chưa được duyệt"
                                        }).OrderByDescending(c => c.CreatedAt).ToList(),
                               approved = g.Where(g => g.Status.Equals("Approved"))
                                         .Select(c => new ClockinAdjustmentRequestDTO
                                         {
                                             RequestId = c.RequestId,
                                             EmployeeId = c.EmployeeId,
                                             ClockInComponentId = c.ClockInComponentId,
                                             RequestedValue = c.RequestedValue,
                                             Message = c.Message,
                                             Attachment = c.Attachment,
                                             Status = c.Status,
                                             ApproverId = c.ApproverId,
                                             Comment = c.Comment,
                                             CreatedAt = c.CreatedAt,
                                             EmployeeName = c.Employee?.EmpName ?? "Không xác định",
                                             ApproverName = c.Approver?.EmpName ?? "Chưa được duyệt"
                                         }).OrderByDescending(c => c.CreatedAt).ToList(),
                               denied = g.Where(g => g.Status.Equals("Denied"))
                                         .Select(c => new ClockinAdjustmentRequestDTO
                                         {
                                             RequestId = c.RequestId,
                                             EmployeeId = c.EmployeeId,
                                             ClockInComponentId = c.ClockInComponentId,
                                             RequestedValue = c.RequestedValue,
                                             Message = c.Message,
                                             Attachment = c.Attachment,
                                             Status = c.Status,
                                             ApproverId = c.ApproverId,
                                             Comment = c.Comment,
                                             CreatedAt = c.CreatedAt,
                                             EmployeeName = c.Employee?.EmpName ?? "Không xác định",
                                             ApproverName = c.Approver?.EmpName ?? "Chưa được duyệt"
                                         }).OrderByDescending(c => c.CreatedAt).ToList()

                           }).ToList();
            return result;
        }

        public async Task<List<IGrouping<int?, ClockInAdjustmentRequest>>> GetGroupByDepIDTest()
        {
            var all = await this._context.ClockInAdjustmentRequests
                                    .Include(c => c.Employee)
                                    .Include(c => c.Approver)
                                    .Include(c => c.ClockInComponent)
                                    .ToListAsync();

            var result = all.GroupBy(c => c.Employee.DepId)
                           .ToList();
            return result;


        }



        public async Task UpdateAsync(ClockInAdjustmentRequest clockInAdjustmentRequest)
        {
            this._context.Update(clockInAdjustmentRequest);
            await this._context.SaveChangesAsync();
        }
    }
}
