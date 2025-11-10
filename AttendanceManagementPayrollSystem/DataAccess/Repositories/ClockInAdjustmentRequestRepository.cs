using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public interface ClockInAdjustmentRequestRepository:BaseRepository
    {
        Task<List<ClockinAdjustmentRequestDTO>> GetByEmpID(int id);
        Task AddAsync(ClockInAdjustmentRequest clockInAdjustmentRequest);
        Task UpdateAsync(ClockInAdjustmentRequest clockInAdjustmentRequest);

        Task<List<ClockinAdjustmentRequestGroupDTO>> GetGroupByDepID();

        Task<List<ClockinAdjustmentRequestGroupDTO>> GetGroupByDepIdAndDateRange(DateTime from, DateTime to);

        Task<List<IGrouping<int?, ClockInAdjustmentRequest>>> GetGroupByDepIDTest();

        Task<ClockInAdjustmentRequest> GetAdjustmentRequestByIdAsync(int id);
    }
}
