using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public class AttendanceServiceImpl : IAttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;

        public AttendanceServiceImpl(IAttendanceRepository attendanceRepository)
        {
            _attendanceRepository = attendanceRepository;
        }

        public List<DepartmentAttendanceDTO> GetDepartmentAttendanceSummary(DateTime? startDate = null, DateTime? endDate = null)
        {
            var data = _attendanceRepository.GetAllClockins(startDate, endDate);

            var result = data
                .Where(c => c.Emp?.Dep != null)
                .GroupBy(c => c.Emp.Dep.DepName)
                .Select(g => new DepartmentAttendanceDTO
                {
                    DepartmentName = g.Key,
                    EmployeeIds = g.Select(x => x.EmpId).Distinct().ToList(),
                    TotalEmployees = g.Select(x => x.EmpId).Distinct().Count(),
                    TotalWorkHours = g.Sum(x => x.WorkUnits ?? 0),
                    TotalScheduledHours = g.Sum(x => x.ScheduledUnits ?? 0),
                    AbsentDays = g.Count(x => !x.WorkHours.HasValue || x.WorkHours.Value == 0)
                })
                .OrderBy(x => x.DepartmentName)
                .ToList();

            return result;
        }
        public List<ClockinDTO> GetAllClockins(DateTime? startDate = null, DateTime? endDate = null)
        {
            var data = _attendanceRepository.GetAllClockins(startDate, endDate);

            // Map Clockin -> ClockinDTO
            return data.Select(c => new ClockinDTO
            {
                CloId = c.CloId,
                EmpId = c.EmpId,
                Date = c.Date,
                WorkUnits = c.WorkUnits,
                ScheduledUnits = c.ScheduledUnits,
                WorkHours = c.WorkHours,
                ScheduledHours = c.ScheduledHours,
                ClockLog = c.ClockLog,
                WorkUnitBreakdown = c.WorkUnitBreakdown
            }).ToList();
        }
    }
}

