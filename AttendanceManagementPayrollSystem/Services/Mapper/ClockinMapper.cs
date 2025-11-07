using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using System.Text.RegularExpressions;

namespace AttendanceManagementPayrollSystem.Services.Mapper
{
    public class ClockinMapper
    {

        //public static ClockinDTO? ToDto(Clockin entity)
        //{
        //    if (entity == null) return null;

        //    string clockLog = entity.ClockLog;
        //    string workUnitBreakdown = entity.WorkUnitBreakdown;

        //    var result = new List<DailyDetailDTO>();

        //    var logParts = clockLog?.Split('|', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();
        //    var unitParts = workUnitBreakdown?.Split('|', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

        //    var logMap = new Dictionary<int, List<string>>();
        //    var unitMap = new Dictionary<int, (List<decimal> actual, List<decimal> scheduled)>();

        //    // Parse ClockLog e.g. 1#7:58,12:01,...
        //    foreach (var part in logParts)
        //    {
        //        var seg = part.Split('#');
        //        if (seg.Length != 2) continue;
        //        if (!int.TryParse(seg[0], out int day)) continue;

        //        var logs = seg[1]
        //            .Split(',', StringSplitOptions.RemoveEmptyEntries)
        //            .Select(x => x.Trim())
        //            .ToList();

        //        logMap[day] = logs;
        //    }

        //    // Parse WorkUnitBreakdown e.g. 1#0.50/0.50,0.50/0.50
        //    foreach (var part in unitParts)
        //    {
        //        var seg = part.Split('#');
        //        if (seg.Length != 2) continue;
        //        if (!int.TryParse(seg[0], out int day)) continue;

        //        var actuals = new List<decimal>();
        //        var scheduled = new List<decimal>();

        //        foreach (var pair in seg[1].Split(',', StringSplitOptions.RemoveEmptyEntries))
        //        {
        //            var nums = pair.Split('/');
        //            if (nums.Length != 2) continue;

        //            if (decimal.TryParse(nums[0], out var a)) actuals.Add(a);
        //            if (decimal.TryParse(nums[1], out var s)) scheduled.Add(s);
        //        }

        //        unitMap[day] = (actuals, scheduled);
        //    }

        //    // Merge
        //    var allDays = logMap.Keys.Union(unitMap.Keys).OrderBy(x => x);
        //    var allDays = logMap.Keys.Union(unitMap.Keys)
        //        .Union(entity.ClockinComponents.Select(c => c.Date.Day))
        //        .OrderBy(x => x);

        //    foreach (var d in allDays)
        //    {
        //        var dto = new DailyDetailDTO
        //        {
        //            Day = d,
        //            Logs = logMap.ContainsKey(d) ? logMap[d] : new List<string>(),
        //            ActualUnit = unitMap.ContainsKey(d) ? unitMap[d].actual.Sum() : 0m,
        //            ScheduledUnit = unitMap.ContainsKey(d) ? unitMap[d].scheduled.Sum() : 0m,
        //            ActualHour = 
        //            ScheduledHour = 
        //        };
        //        result.Add(dto);
        //    }

        //    return new ClockinDTO
        //    {
        //        CloId = entity.CloId,
        //        EmpId = entity.EmpId,
        //        Date = entity.Date,
        //        WorkUnits = entity.WorkUnits,
        //        ScheduledUnits = entity.ScheduledUnits,
        //        WorkHours = entity.WorkHours,
        //        ScheduledHours = entity.ScheduledHours,
        //        ClockLog = entity.ClockLog,
        //        WorkUnitBreakdown = entity.WorkUnitBreakdown,
        //        DailyRecords = result,
        //        Components = entity.ClockinComponents.Select(c => ToComponentDto(c)).ToList()
        //    };
        //}

        public static ClockinDTO? ToDto(Clockin entity)
        {
            if (entity == null)
                return null;

            // group by day
            var grouped = entity.ClockinComponents
                .Where(c => c.Date.HasValue)
                .GroupBy(c => c.Date.Value.Day)
                .OrderBy(g => g.Key);

            var dailyRecords = new List<DailyDetailDTO>();

            foreach (var g in grouped)
            {
                decimal totalWorkHours = g.Sum(x => x.WorkHours ?? 0m);
                decimal totalScheduledHours = g.Sum(x => x.ScheduledHours ?? 0m);
                decimal totalWorkUnits = g.Sum(x => x.WorkUnits ?? 0m);
                decimal totalScheduledUnits = g.Sum(x => x.ScheduledUnits ?? 0m);

                dailyRecords.Add(new DailyDetailDTO
                {
                    Day = g.Key,
                    Logs = g.Select(x => x.ClockinLog).Where(l => !string.IsNullOrWhiteSpace(l)).ToList(),
                    ActualHour = totalWorkHours,
                    ScheduledHour = totalScheduledHours,
                    ActualUnit = totalWorkUnits,
                    ScheduledUnit = totalScheduledUnits
                });
            }

            return new ClockinDTO
            {
                CloId = entity.CloId,
                EmpId = entity.EmpId,
                Date = entity.Date,
                WorkUnits = entity.ClockinComponents.Sum(x => x.WorkUnits ?? 0m),
                ScheduledUnits = entity.ClockinComponents.Sum(x => x.ScheduledUnits ?? 0m),
                WorkHours = entity.ClockinComponents.Sum(x => x.WorkHours ?? 0m),
                ScheduledHours = entity.ClockinComponents.Sum(x => x.ScheduledHours ?? 0m),
                ClockLog = entity.ClockLog,
                WorkUnitBreakdown = entity.WorkUnitBreakdown,
                DailyRecords = dailyRecords,
                Components = entity.ClockinComponents.Select(ToComponentDto).ToList()
            };
        }

        public static Clockin? ToEntity(ClockinDTO dto)
        {
            if (dto == null) return null;

            return new Clockin
            {
                CloId = dto.CloId,
                EmpId = dto.EmpId,
                Date = dto.Date,
                WorkUnits = dto.WorkUnits,
                ScheduledUnits = dto.ScheduledUnits,
                WorkHours = dto.WorkHours,
                ScheduledHours = dto.ScheduledHours,
                ClockLog = dto.ClockLog,
                WorkUnitBreakdown = dto.WorkUnitBreakdown,
                ClockinComponents = dto.Components.Select(c => ToComponentEntity(c)).ToList()
            };
        }

        public static ClockinComponent? ToComponentEntity(ClockinComponentDto dto)
        {
            if (dto == null) return null;

            return new ClockinComponent
            {
                Id = dto.Id,
                Shift = dto.Shift,
                Date = dto.Date,
                CheckIn = dto.CheckIn,
                CheckOut = dto.CheckOut,
                ClockinLog = dto.ClockinLog,
                Description = dto.Description,
                WorkHours = dto.WorkHours,
                ScheduledHours = dto.ScheduledHours,
                WorkUnits = dto.WorkUnits,
                ScheduledUnits = dto.ScheduledUnits
            };

        }
    
        public static ClockinComponentDto ToComponentDto(ClockinComponent component)
        {
            if (component == null) return null;

            return new ClockinComponentDto
            {
                Id = component.Id,
                Shift = component.Shift,
                Date = component.Date,
                CheckIn = component.CheckIn,
                CheckOut = component.CheckOut,
                ClockinLog = component.ClockinLog,
                Description = component.Description,
                WorkHours = component.WorkHours,
                ScheduledHours = component.ScheduledHours,
                WorkUnits = component.WorkUnits,
                ScheduledUnits = component.ScheduledUnits,
                OverridedWorkunits=component.OverridedWorkunits
            };
        }
    }
}
