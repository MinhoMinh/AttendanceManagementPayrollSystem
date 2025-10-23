using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace AttendanceManagementPayrollSystem.Services
{
    public class ShiftServiceImpl : ShiftService
    {
        private readonly ShiftRepository _shiftRepo;

        public ShiftServiceImpl(ShiftRepository shiftRepo)
        {
            _shiftRepo = shiftRepo;
        }

        public async Task<WeeklyShiftDto?> GetWeeklyShiftDto(int empId)
        {
            var weeklyShift = await _shiftRepo.GetWeeklyShift(empId);

            return ToWeeklyDTO(weeklyShift);
        }

        public async Task<Dictionary<int, WeeklyShiftDto?>> GetWeeklyShiftDtos(IEnumerable<int> empIds)
        {
            var shiftDictionary = await _shiftRepo.GetWeeklyShifts(empIds);

            Dictionary<int, WeeklyShiftDto?> result = new();

            foreach (var kvp in shiftDictionary)
            {
                result.Add(kvp.Key, ToWeeklyDTO(kvp.Value));
            }

            return result;
        }

        private WeeklyShiftDto? ToWeeklyDTO(WeeklyShift? shift)
        {
            if (shift == null)
                return null;

            var dto = new WeeklyShiftDto
            {
                ShiftId = shift.ShiftId,
                ShiftName = shift.ShiftName,
                ShiftDescription = shift.ShiftDescription
            };

            if (shift.MonDailyShift != null)
                dto.MonDailyShift = new DailyShiftDto { ShiftDtos = ToDailyDto(shift.MonDailyShift.ShiftString, shift.MonDailyShift.ShiftHours) };

            if (shift.TueDailyShift != null)
                dto.TueDailyShift = new DailyShiftDto { ShiftDtos = ToDailyDto(shift.TueDailyShift.ShiftString, shift.TueDailyShift.ShiftHours) };

            if (shift.WedDailyShift != null)
                dto.WedDailyShift = new DailyShiftDto { ShiftDtos = ToDailyDto(shift.WedDailyShift.ShiftString, shift.WedDailyShift.ShiftHours) };

            if (shift.ThuDailyShift != null)
                dto.ThuDailyShift = new DailyShiftDto { ShiftDtos = ToDailyDto(shift.ThuDailyShift.ShiftString, shift.ThuDailyShift.ShiftHours) };

            if (shift.FriDailyShift != null)
                dto.FriDailyShift = new DailyShiftDto { ShiftDtos = ToDailyDto(shift.FriDailyShift.ShiftString, shift.FriDailyShift.ShiftHours) };

            if (shift.SatDailyShift != null)
                dto.SatDailyShift = new DailyShiftDto { ShiftDtos = ToDailyDto(shift.SatDailyShift.ShiftString, shift.SatDailyShift.ShiftHours) };

            if (shift.SunDailyShift != null)
                dto.SunDailyShift = new DailyShiftDto { ShiftDtos = ToDailyDto(shift.SunDailyShift.ShiftString, shift.SunDailyShift.ShiftHours) };

            return dto;
        }


        private List<ShiftDto> ToDailyDto(string shiftString, string shiftHours) {

            var result = new List<ShiftDto>();
            if (string.IsNullOrWhiteSpace(shiftString))
                return result;

            var segments = shiftString.Split('|', StringSplitOptions.RemoveEmptyEntries);
            var hourSegments = (shiftHours ?? "")
                .Split('|', StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < segments.Length; i++)
            {
                var segment = segments[i];
                var timeAndWork = segment.Split('#', StringSplitOptions.RemoveEmptyEntries);
                if (timeAndWork.Length < 2)
                    continue;

                var times = timeAndWork[0].Split('-', StringSplitOptions.RemoveEmptyEntries);
                if (times.Length != 2)
                    continue;

                if (TimeSpan.TryParse(times[0], out var start) &&
                    TimeSpan.TryParse(times[1], out var end))
                {
                    var match = Regex.Match(timeAndWork[1], @"\d+(\.\d+)?");
                    var workStr = match.Value;

                    if (decimal.TryParse(workStr, out var work))
                    {
                        decimal workHours = 0;
                        if (i < hourSegments.Length &&
                            decimal.TryParse(hourSegments[i], out var hours))
                            workHours = hours;

                        result.Add(new ShiftDto
                        {
                            StartTime = start,
                            EndTime = end,
                            WorkUnits = work,
                            WorkHours = workHours
                        });
                    }
                }
            }

                return result;

        }
    }
}
