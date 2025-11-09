using System.Collections.Generic;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public interface IOvertimeService
    {
        // 🔹 Lấy lịch sử OT theo nhân viên với filter thời gian
        Task<IEnumerable<OvertimeRequestDTO>> GetOvertimeHistoryByEmployee(int empId, DateOnly? startDate, DateOnly? endDate);

        // 🔹 Lấy 1 OT request
        OvertimeRequest GetById(int id);

        // 🔹 Nhân viên tự tạo yêu cầu OT
        void CreateOvertimeRequest(OvertimeRequest request);

        // 🔹 Trưởng phòng tạo yêu cầu OT cho nhân viên
        void CreateOvertimeRequestByHead(OvertimeRequest request);

        // 🔹 Duyệt OT
        void ApproveOvertimeRequest(int reqId, int approverId);

        // 🔹 Từ chối OT
        void RejectOvertimeRequest(int reqId, int approverId);

        //Task<IGrouping<int, OvertimeRequestDTO>> GetOvertimeHistoryByHead(int headId, DateOnly? startDate, DateOnly? endDate);


        IEnumerable<OvertimeRateDTO> GetRates();
    }

}
