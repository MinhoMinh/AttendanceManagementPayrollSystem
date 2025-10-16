using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.Helper;
using System;
using System.Data;
using System.Globalization;
using System.Text;
using static AttendanceManagementPayrollSystem.DTO.WeeklyShiftDto;

namespace AttendanceManagementPayrollSystem.Services
{
    public class ClockinServiceImpl : ClockinService
    {
        private readonly EmployeeRepository _empRepo;
        private readonly ClockinRepository _clockinRepo;
        private readonly ShiftService _shiftService;

        public ClockinServiceImpl(EmployeeRepository employeeRepo, ClockinRepository clockinRepo, ShiftService shiftService)
        {
            _empRepo = employeeRepo;
            _clockinRepo = clockinRepo;
            _shiftService = shiftService;
        }

        public async Task<List<ClockinDTO>?> ReadClockinData(DataTable table)
        {
            // Parse month and year
            DateTime startDate = DateTime.Now;
            var cellValue = table.Rows[1][2]?.ToString()?.Trim(); // C2
            if (!string.IsNullOrEmpty(cellValue))
            {
                var parts = cellValue.Split('~', StringSplitOptions.TrimEntries);
                if (parts.Length == 2 &&
                    DateTime.TryParseExact(parts[0], "dd-MM-yyyy", null, System.Globalization.DateTimeStyles.None, out startDate))
                {

                    Console.WriteLine($"Month: {startDate.Month}, Year: {startDate.Year}");
                }
                else
                {
                    return null;
                }
            }

            List<ClockinDTO> clokinList = new();


            for (int index = 2; index < table.Rows.Count - 3; index += 4)
            {
                ClockinDTO clockin = new ClockinDTO();
                StringBuilder clockLogBuilder = new StringBuilder();
                clockin.Date = startDate;

                var idCell = table.Rows[index][2]?.ToString();
                if (!int.TryParse(idCell, out int clockId))
                    continue;

                var row1 = table.Rows[index + 1];
                var row2 = table.Rows[index + 3];

                // stop if row1 is blank (no data at all)
                bool row1Empty = row1.ItemArray.All(c => string.IsNullOrWhiteSpace(c?.ToString()));
                if (row1Empty)
                    break;

                Console.WriteLine($"Clock ID: {clockId}");


                var empId = await _empRepo.GetIdByClockId(clockId);

                if (empId == -1) continue;
                else clockin.EmpId = empId;

                int lastCol = 30; // AE = index 30

                for (int col = 0; col <= lastCol && col < row1.ItemArray.Length; col++)
                {
                    DailyDetailDTO dayDTO = new DailyDetailDTO();

                    var raw1 = row1[col]?.ToString() ?? "";
                    var raw2 = row2[col]?.ToString() ?? "";

                    string val1;
                    if (int.TryParse(raw1, out int day))
                    {
                        dayDTO.Day = day;
                        if (day > 1) clockLogBuilder.Append('|');
                        clockLogBuilder.Append(day.ToString());
                        clockLogBuilder.Append('#');
                    }
                    else
                        break;

                    var normalized = raw2.Replace("\r\n", ",").Replace("\n", ",");
                    var tokens = normalized.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                    var validTimes = new List<string>();
                    var timeFormats = new[] { "hh\\:mm", "h\\:mm" };

                    foreach (var t in tokens)
                    {
                        var tok = t.Trim();
                        dayDTO.Logs.Add(tok);
                        //if (TimeSpan.TryParseExact(tok, timeFormats, CultureInfo.InvariantCulture, out var _))
                            validTimes.Add(tok);
                    }

                    string val2 = string.Join(",", validTimes);
                    //clockLogBuilder.Append(val2);
                    clockLogBuilder.Append(val2);

                    Console.WriteLine($"Col {col}: Row+1={raw1}, Row+3={val2}");

                    clockin.DailyRecords.Add(dayDTO);
                }

                clockin.ClockLog = clockLogBuilder.ToString();

                clokinList.Add(clockin);
                Console.WriteLine();
            }

            var shiftDictionary = await _shiftService.GetWeeklyShiftDtos(clokinList.Select(c => c.EmpId));


            foreach (var clockin in clokinList) //an employee
            {
                var weeklyShift = shiftDictionary[clockin.EmpId]; //weekly shift of the emp

                if (weeklyShift == null) continue;

                StringBuilder workUnitBreakdownBuilder = new StringBuilder();

                foreach (var dayDetail in clockin.DailyRecords) //loop through each day of clockin 
                {
                    DateTime date = new DateTime(clockin.Date.Year, clockin.Date.Month, dayDetail.Day);

                    var dailyShift = weeklyShift.GetDailyShift(date.DayOfWeek); //get shift of that day

                    if (dailyShift == null) continue;

                    if (dayDetail.Day > 1) workUnitBreakdownBuilder.Append('|');
                    workUnitBreakdownBuilder.Append(dayDetail.Day);
                    workUnitBreakdownBuilder.Append('#');

                    List<ShiftSection> sections = new();

                    foreach (var shift in dailyShift.ShiftDtos)
                        sections.Add(new ShiftSection(shift.StartTime, shift.EndTime, shift.Workhour));//convert to sec

                    foreach (var log in dayDetail.Logs)
                    {
                        foreach (var section in sections)
                        {
                            if (section.IsCheckInSection(TimeSpan.Parse(log)))
                                break;
                        }
                    }

                    for (int i = 0; i < sections.Count; i++)
                    {
                        decimal actual = sections[i].CalculateWorkhour();
                        decimal expected = sections[i].ExpectedWorkhour;
                        dayDetail.Actual.Add(actual);
                        dayDetail.Scheduled.Add(expected);

                        clockin.WorkUnits ??= 0;
                        clockin.WorkUnits += actual;
                        clockin.ScheduledUnits ??= 0;
                        clockin.ScheduledUnits += expected;

                        if (i >= 1) { workUnitBreakdownBuilder.Append(','); }
                        workUnitBreakdownBuilder.AppendFormat("{0:F2}/{1:F2}", actual, expected);
                    }
                }

                clockin.WorkUnitBreakdown = workUnitBreakdownBuilder.ToString();
            }

            return clokinList;
        }

        public async Task SaveClockinData(List<ClockinDTO> dtos)
        {
            await _clockinRepo.SaveClockinData(dtos.Select(dto => ToModel(dto)));
        }

        private Clockin ToModel(ClockinDTO dto) {
            return new Clockin
            {
                CloId = dto.CloId,
                EmpId = dto.EmpId,
                Date = dto.Date,
                WorkUnits = dto.WorkUnits,
                ScheduledUnits = dto.ScheduledUnits,
                ClockLog = dto.ClockLog,
                WorkUnitBreakdown = dto.WorkUnitBreakdown
            };
        }
    }
}
