using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.Mapper
{
    public class OvertimeMapper
    {
        public static OvertimeRequest ToEntity(OvertimeRequestDTO o)
        {
            return new OvertimeRequest
            {
                ReqId = o.ReqId,
                ReqDate = o.ReqDate,
                StartTime = o.StartTime,
                EndTime = o.EndTime,
                Hours = o.Hours,
                OvertimeType = o.OvertimeType,
                Status = o.Status,
                Reason = o.Reason,
                ApprovedDate = o.ApprovedDate,
                ApprovedBy = o.ApprovedBy,
                CreatedDate = o.CreatedDate,
                LinkedPayrollRunId = o.LinkedPayRunId
            };
        }

        public static OvertimeRequestDTO ToDTO(OvertimeRequest o)
        {
            return new OvertimeRequestDTO
            {
                ReqId = o.ReqId,
                ReqDate = o.ReqDate,
                StartTime = o.StartTime,
                EndTime = o.EndTime,
                Hours = o.Hours,
                OvertimeType = o.OvertimeType,
                OvertimeTypeName = o.OvertimeTypeNavigation?.OvertimeType ?? "-",
                Status = o.Status,
                Reason = o.Reason,
                ApprovedByName = o.ApprovedByNavigation?.EmpName ?? "-",
                ApprovedBy = o.ApprovedBy,
                ApprovedDate = o.ApprovedDate,
                CreatedDate = o.CreatedDate,
                LinkedPayRunId = o.LinkedPayrollRunId,
                LinkedPayRunName = o.LinkedPayrollRun?.Name ?? "-"
            };
        }
    }
}
