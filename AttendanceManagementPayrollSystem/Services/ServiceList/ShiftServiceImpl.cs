using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
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


        private List<ShiftDto> ToDailyDto(string shiftString, string shiftHours)
        {

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

        public async Task<DailyShiftAfterCreateDTO> CreateAsync(DailyShiftCreateDTO dto)
        {
            string shiftString=string.Join("|", dto.Segments.Select(s=>$"{s.StartTime:hh\\:mm}-{s.EndTime:hh\\:mm}#{s.WorkUnit}c"));

            decimal totalWorkUnit = dto.Segments.Sum(s => s.WorkUnit);

            string shiftHours = string.Join("|", dto.Segments.Select(s => (s.EndTime - s.StartTime).TotalHours.ToString("0.00")));

            var entity = new DailyShift
            {
                ShiftName = dto.ShiftName,
                ShiftDescription = dto.ShiftDescription,
                ShiftString = shiftString,
                ShiftWorkUnit = totalWorkUnit,
                ShiftHours = shiftHours
            };

            await this._shiftRepo.AddShiftAsync(entity);

            return new DailyShiftAfterCreateDTO
            {
                ShiftId = entity.ShiftId,
                ShiftName = entity.ShiftName,
                ShiftDescription = entity.ShiftDescription,
                ShiftString = entity.ShiftString,
                ShiftWorkUnit = entity.ShiftWorkUnit,
                ShiftHours = shiftHours
            };
        }

        public async Task<List<DailyShiftViewDTO>> GetAllForViewAsync()
        {
            var shifts = await this._shiftRepo.GetAllDailyShiftAsync();
            var result = new List<DailyShiftViewDTO>();

            foreach (var s in shifts)
            {
                // Tách ShiftString thành segments
                var shiftHoursArray = s.ShiftHours?.Split('|', StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

                var segments = s.ShiftString.Split('|', StringSplitOptions.RemoveEmptyEntries)
                    .Select((seg, index) =>
                    {
                        var parts = seg.Split('#');
                        var timeRange = parts[0].Split('-');
                        var workUnit = decimal.Parse(parts[1].Replace("c", ""));
                        var workHours = index < shiftHoursArray.Length ? decimal.Parse(shiftHoursArray[index]) : 0;

                        return new ShiftDto
                        {
                            StartTime = TimeSpan.Parse(timeRange[0]),
                            EndTime = TimeSpan.Parse(timeRange[1]),
                            WorkUnits = workUnit,
                            WorkHours = workHours
                        };
                    })
                    .ToList();

                result.Add(new DailyShiftViewDTO
                {
                    ShiftId = s.ShiftId,
                    ShiftName = s.ShiftName,
                    ShiftDescription = s.ShiftDescription,
                    ShiftString = s.ShiftString,
                    ShiftWorkUnit = s.ShiftWorkUnit,
                    Segments = segments
                });
            }

            return result;
        }

        public async Task<DailyShiftViewDTO?> GetByIdAsync(int id)
        {
            var s = await _shiftRepo.GetDailyShiftByIdAsync(id);
            if (s == null) return null;

            // Tách ShiftHours từ DB
            var shiftHoursArray = s.ShiftHours?.Split('|', StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

            // Parse ShiftString -> Segments
            var segments = s.ShiftString.Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select((seg, index) =>
                {
                    var parts = seg.Split('#');
                    var timeRange = parts[0].Split('-');
                    var workUnit = decimal.Parse(parts[1].Replace("c", ""));
                    var workHours = index < shiftHoursArray.Length ? decimal.Parse(shiftHoursArray[index]) : 0;

                    return new ShiftDto
                    {
                        StartTime = TimeSpan.Parse(timeRange[0]),
                        EndTime = TimeSpan.Parse(timeRange[1]),
                        WorkUnits = workUnit,
                        WorkHours = workHours
                    };
                }).ToList();

            return new DailyShiftViewDTO
            {
                ShiftId = s.ShiftId,
                ShiftName = s.ShiftName,
                ShiftDescription = s.ShiftDescription,
                ShiftString = s.ShiftString,
                ShiftWorkUnit = s.ShiftWorkUnit,
                Segments = segments
            };
        }

        public async Task<List<WeeklyShiftViewDTO>> GetAllWeeklyShiftAsync()
        {
            var shifts = await _shiftRepo.GetAllWeeklyShiftAsync();

            return shifts.Select(w => new WeeklyShiftViewDTO
            {
                ShiftId = w.ShiftId,
                ShiftName = w.ShiftName,
                ShiftDescription = w.ShiftDescription,

                MonDailyShift = MapDailyShiftView(w.MonDailyShift),
                TueDailyShift = MapDailyShiftView(w.TueDailyShift),
                WedDailyShift = MapDailyShiftView(w.WedDailyShift),
                ThuDailyShift = MapDailyShiftView(w.ThuDailyShift),
                FriDailyShift = MapDailyShiftView(w.FriDailyShift),
                SatDailyShift = MapDailyShiftView(w.SatDailyShift),
                SunDailyShift = MapDailyShiftView(w.SunDailyShift)
            }).ToList();
        }

        private static DailyShiftViewDTO? MapDailyShiftView(DailyShift? d)
        {
            if (d == null) return null;

            // Parse ShiftString và ShiftHours nếu có
            var shiftHoursArray = d.ShiftHours?.Split('|', StringSplitOptions.RemoveEmptyEntries) ?? Array.Empty<string>();

            var segments = (d.ShiftString ?? string.Empty)
                .Split('|', StringSplitOptions.RemoveEmptyEntries)
                .Select((seg, index) =>
                {
                    var parts = seg.Split('#');
                    var timeRange = parts[0].Split('-');
                    var workUnit = decimal.Parse(parts[1].Replace("c", ""));
                    var workHours = index < shiftHoursArray.Length ? decimal.Parse(shiftHoursArray[index]) : 0;

                    return new ShiftDto
                    {
                        StartTime = TimeSpan.Parse(timeRange[0]),
                        EndTime = TimeSpan.Parse(timeRange[1]),
                        WorkUnits = workUnit,
                        WorkHours = workHours
                    };
                }).ToList();

            return new DailyShiftViewDTO
            {
                ShiftId = d.ShiftId,
                ShiftName = d.ShiftName,
                ShiftDescription = d.ShiftDescription,
                ShiftString = d.ShiftString,
                ShiftWorkUnit = d.ShiftWorkUnit,
                Segments = segments
            };
        }

        public async Task UpdateWeeklyShiftAsync(int id, WeeklyShiftCreateUpdateDTO dto)
        {
            var entity = await this._shiftRepo.GetWeeklyShiftById(id);
            if(entity==null) throw new Exception("Weekly shift not found");

            entity.ShiftName = dto.ShiftName;
            entity.ShiftDescription = dto.ShiftDescription;

            entity.MonDailyShiftId = dto.MonDailyShiftId;
            entity.TueDailyShiftId = dto.TueDailyShiftId;
            entity.WedDailyShiftId = dto.WedDailyShiftId;
            entity.ThuDailyShiftId = dto.ThuDailyShiftId;
            entity.FriDailyShiftId = dto.FriDailyShiftId;
            entity.SatDailyShiftId = dto.SatDailyShiftId;
            entity.SunDailyShiftId = dto.SunDailyShiftId;

            await this._shiftRepo.UpdateWeeklyShiftAsync(entity);
        }
    }
}
