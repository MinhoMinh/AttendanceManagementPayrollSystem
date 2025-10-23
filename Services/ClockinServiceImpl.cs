using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services.Helper;
using AttendanceManagementPayrollSystem.Services.Mapper;
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

        public async Task<IEnumerable<ClockinDTO>> GetByEmployeeAsync(int empId, DateTime startDate, int months)
        {
            var clockin = await _clockinRepo.GetByEmployeeAsync(empId, startDate, months);

            if (clockin == null) return null;
            else return clockin.Select(c => ClockinMapper.ToDto(c));
        }

        public async Task<ClockinDTO?> GetClockinsByEmployeeAsync(int empId, int month, int year)
        {
            var clockin = await _clockinRepo.GetByEmployeeAndMonthAsync(empId, month, year);

            if (clockin == null) return null;
            else return ClockinMapper.ToDto(clockin);
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
                    List<ClockinComponentDto> components = new();

                    foreach (var shift in dailyShift.ShiftDtos)
                    {
                        sections.Add(new ShiftSection(shift.StartTime, shift.EndTime, shift.WorkUnits, shift.WorkHours));//convert to section
                        components.Add(new ClockinComponentDto 
                        { 
                            Date = date,
                            Shift = shift.ToString()
                        });
                    }

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
                        decimal actualWorkUnits = Math.Round(sections[i].CalculateWorkUnits(), 2);
                        decimal expectedWorkUnits = Math.Round(sections[i].ExpectedWorkUnits, 2);
                        decimal actualWorkHours = Math.Round(sections[i].CalculateWorkHours(), 2);
                        decimal expectedWorkHours = Math.Round(sections[i].ExpectedWorkHours, 2);

                        dayDetail.ActualUnit = (dayDetail.ActualUnit ?? 0m) + actualWorkUnits;
                        dayDetail.ScheduledUnit = (dayDetail.ScheduledUnit ?? 0m) + expectedWorkUnits;
                        dayDetail.ActualHour = (dayDetail.ActualHour ?? 0m) + actualWorkHours;
                        dayDetail.ScheduledHour = (dayDetail.ScheduledHour ?? 0m) + expectedWorkUnits;

                        clockin.WorkUnits ??= 0;
                        clockin.WorkUnits += actualWorkUnits;
                        clockin.ScheduledUnits ??= 0;
                        clockin.ScheduledUnits += expectedWorkUnits;
                        clockin.WorkHours ??= 0;
                        clockin.WorkHours += actualWorkHours;
                        clockin.ScheduledHours ??= 0;
                        clockin.ScheduledHours += expectedWorkHours;

                        if (i >= 1) { workUnitBreakdownBuilder.Append(','); }
                        workUnitBreakdownBuilder.AppendFormat("{0:F2}/{1:F2}", actualWorkUnits, expectedWorkUnits);

                        var span = sections[i].GetCheckIn();
                        if (span != TimeSpan.FromHours(-20))
                        {
                            components[i].CheckIn = new DateTime(date.Year, date.Month, date.Day, span.Hours, span.Minutes, 0);
                        }

                        var span2 = sections[i].GetCheckOut();
                        if (span2 != TimeSpan.FromHours(-20))
                        {
                            components[i].CheckOut = new DateTime(date.Year, date.Month, date.Day, span2.Hours, span2.Minutes, 0);
                        }
                        components[i].WorkUnits = actualWorkUnits;
                        components[i].ScheduledUnits = expectedWorkUnits;
                        components[i].WorkHours = sections[i].CalculateWorkHours();
                        components[i].ScheduledHours = sections[i].ExpectedWorkHours;
                        components[i].ClockinLog = sections[i].ToLogString();
                        components[i].Description = sections[i].GetDescription();

                    }

                    Console.WriteLine($"daily {dayDetail.Day} check {dayDetail.ActualUnit} + {dayDetail.ScheduledUnit}");
                    Console.WriteLine($"daily {dayDetail.Day} check {dayDetail.ActualHour} + {dayDetail.ScheduledHour}");

                    clockin.Components.AddRange(components);

                }

                clockin.WorkUnitBreakdown = workUnitBreakdownBuilder.ToString();
            }

            return clokinList;
        }

        public async Task SaveClockinData(List<ClockinDTO> dtos)
        {
            await _clockinRepo.SaveClockinData(dtos);
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
