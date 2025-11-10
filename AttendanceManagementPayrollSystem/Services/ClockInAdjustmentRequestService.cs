using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface ClockInAdjustmentRequestService
    {
        Task<bool> AddClockInAdjustmentRequest(ClockinAdjustmentRequestCreateDTO dto);

        Task<List<IGrouping<int?, ClockInAdjustmentRequest>>> GetGroupByDepIDTest();

        Task<List<ClockinAdjustmentRequestGroupDTO>> GetClockinAdjustmentRequestGroupByDepID();

        Task<List<ClockinAdjustmentRequestGroupDTO>> GetClockinAdjustmentRequestGroupByDepIdAndDateRange(DateTime from, DateTime to);

        Task<bool> RespondAsync(ClockinAdjustmentRespondDTO dto);
    }
}
