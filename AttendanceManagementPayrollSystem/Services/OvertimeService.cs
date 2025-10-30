using System;
using System.Collections.Generic;
using System.Linq;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.Interfaces;
using AttendanceManagementPayrollSystem.DataAccess.Repositories;

namespace AttendanceManagementPayrollSystem.Services
{
    public class OvertimeService : IOvertimeService
    {
        private readonly IOvertimeRepository _repo;

        public OvertimeService(IOvertimeRepository repo)
        {
            _repo = repo;
        }

        // 🔹 Lấy lịch sử OT theo nhân viên
        public IEnumerable<OvertimeRequestDTO> GetOvertimeHistoryByEmployee(int empId)
        {
            return _repo.GetOvertimeByEmployeeId(empId)
                .Select(o => new OvertimeRequestDTO
                {
                    ReqId = o.ReqId,
                    ReqDate = o.ReqDate,
                    StartTime = o.StartTime,
                    EndTime = o.EndTime,
                    Hours = o.Hours,
                    OvertimeType = o.OvertimeTypeNavigation?.OvertimeType ?? "-",
                    Status = o.Status,
                    Reason = o.Reason,
                    ApprovedByName = o.ApprovedByNavigation?.EmpName ?? "-",
                    ApprovedDate = o.ApprovedDate
                });
        }

        // 🔹 Lấy 1 OT request
        public OvertimeRequest GetById(int id)
        {
            return _repo.GetById(id);
        }

        // 🔹 Tạo yêu cầu mới
        public void CreateOvertimeRequest(OvertimeRequest request)
        {
            _repo.Create(request);
        }

        // 🔹 Duyệt yêu cầu OT
        public void ApproveOvertimeRequest(int reqId, int approverId)
        {
            _repo.Approve(reqId, approverId);
        }

        // 🔹 Từ chối yêu cầu OT
        public void RejectOvertimeRequest(int reqId, int approverId)
        {
            _repo.Reject(reqId, approverId);
        }
    }
}
