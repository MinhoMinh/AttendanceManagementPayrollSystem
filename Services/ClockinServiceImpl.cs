using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using System;

namespace AttendanceManagementPayrollSystem.Services
{
    public class ClockinServiceImpl : ClockinService
    {
        private readonly ClockinRepository _clockinRepo;

        public ClockinServiceImpl(ClockinRepository clockinRepo)
        {
            _clockinRepo = clockinRepo;
        }


        // Parse WorkUnitBreakdown + ClockLog thành danh sách DailyDetailDTO
        public List<DailyDetailDTO> ParseClockData(string clockLog, string breakdown)
        {
            var result = new List<DailyDetailDTO>();

            // ===== Parse WorkUnitBreakdown =====
            var breakdownDict = new Dictionary<int, (decimal Actual, decimal Scheduled)>();
            if (!string.IsNullOrWhiteSpace(breakdown))
            {
                var parts = breakdown.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    var split = p.Split('#');
                    if (split.Length != 2) continue;

                    int day = int.Parse(split[0]);
                    var pairs = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                    decimal actual = 0, scheduled = 0;

                    foreach (var pair in pairs)
                    {
                        var vs = pair.Split('/');
                        if (vs.Length == 2 &&
                            decimal.TryParse(vs[0], out var a) &&
                            decimal.TryParse(vs[1], out var s))
                        {
                            actual += a;
                            scheduled += s;
                        }
                    }

                    breakdownDict[day] = (actual, scheduled);
                }
            }

            // ===== Parse ClockLog =====
            var clockDict = new Dictionary<int, List<string>>();
            if (!string.IsNullOrWhiteSpace(clockLog))
            {
                var parts = clockLog.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var p in parts)
                {
                    var split = p.Split('#');
                    if (split.Length != 2) continue;

                    int day = int.Parse(split[0]);
                    var times = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                                        .Select(t => t.Trim())
                                        .ToList();
                    clockDict[day] = times;
                }
            }

            // ===== Gộp lại theo ngày =====
            var allDays = breakdownDict.Keys.Union(clockDict.Keys).OrderBy(d => d);
            foreach (var day in allDays)
            {
                breakdownDict.TryGetValue(day, out var work);
                clockDict.TryGetValue(day, out var times);

                result.Add(new DailyDetailDTO
                {
                    Day = day,
                    Actual = work.Actual,
                    Scheduled = work.Scheduled,
                    Logs = times ?? new List<string>()
                });
            }

            return result;
        }

        // ===== Lấy toàn bộ clockin theo nhân viên, kèm dữ liệu DailyRecords =====
        public async Task<List<ClockinDTO>> GetClockinsByEmployeeAsync(int empId, int month, int year)
        {
            var clockins = await _clockinRepo.GetByEmployeeAndMonthAsync(empId, month, year);
            var list = new List<ClockinDTO>();

            foreach (var c in clockins)
            {
                var dto = new ClockinDTO
                {
                    CloId = c.CloId,
                    EmpId = c.EmpId,
                    Date = c.Date,
                    WorkUnits = c.WorkUnits,
                    ScheduledUnits = c.ScheduledUnits,
                    ClockLog = c.ClockLog,
                    WorkUnitBreakdown = c.WorkUnitBreakdown,
                    DailyRecords = ParseClockData(c.ClockLog, c.WorkUnitBreakdown)
                };

                list.Add(dto);
            }

            return list;
        }
    
        
    }
}
