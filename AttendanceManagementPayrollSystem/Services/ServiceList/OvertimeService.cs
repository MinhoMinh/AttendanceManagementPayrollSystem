using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.Mapper;
using AttendanceManagementPayrollSystem.Services.ServiceList;

public class OvertimeService : IOvertimeService
{
    private readonly IOvertimeRepository _repo;

    public OvertimeService(IOvertimeRepository repo)
    {
        _repo = repo;
    }

    // 🔹 Lấy lịch sử OT theo nhân viên, có filter start/end
    public async Task<IEnumerable<OvertimeRequestDTO>> GetOvertimeHistoryByEmployee(int empId, DateOnly? startDate, DateOnly? endDate)
    {
        var data = await _repo.GetOvertimeByEmployeeId(empId, startDate, endDate);

        return data.Select(OvertimeMapper.ToDTO);
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

    //public async Task<IGrouping<int, OvertimeRequestDTO>> GetOvertimeHistoryByHead(int headId, DateOnly? startDate, DateOnly? endDate)
    //{
    //    var data = await _repo.GetOvertimeByHeadId(headId, startDate, endDate);

    //    return data.Select(OvertimeMapper.ToDTO);
    //}

    public IEnumerable<OvertimeRateDTO> GetRates()
    {
        var data = _repo.GetRates()
            .Select(x => new OvertimeRateDTO
            {
                Id = x.Id,
                OvertimeType = x.OvertimeType,
                RateMultiplier = x.RateMultiplier,
                EffectiveDate = x.EffectiveDate,
                CreatedBy = x.CreatedBy
            })
            .ToList();

        return data;
    }

}
