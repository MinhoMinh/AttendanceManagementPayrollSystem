using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.Interfaces;

public class OvertimeService : IOvertimeService
{
    private readonly IOvertimeRepository _repo;

    public OvertimeService(IOvertimeRepository repo)
    {
        _repo = repo;
    }

    // 🔹 Lấy lịch sử OT theo nhân viên, có filter start/end
    public IEnumerable<OvertimeRequestDTO> GetOvertimeHistoryByEmployee(int empId, DateOnly? startDate, DateOnly? endDate)
    {
        var data = _repo.GetOvertimeByEmployeeId(empId);

        if (startDate.HasValue)
            data = data.Where(o => o.ReqDate >= startDate.Value);

        if (endDate.HasValue)
            data = data.Where(o => o.ReqDate <= endDate.Value);

        return data.Select(o => new OvertimeRequestDTO
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

    // 🔹 Tạo yêu cầu mới (nhân viên tự request)
    public void CreateOvertimeRequest(OvertimeRequest request)
    {
        _repo.Create(request);
    }

    // 🔹 Tạo yêu cầu OT do trưởng phòng tạo hộ
    public void CreateOvertimeRequestByHead(OvertimeRequest request)
    {
        // logic có thể thêm thông tin headId nếu cần
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
