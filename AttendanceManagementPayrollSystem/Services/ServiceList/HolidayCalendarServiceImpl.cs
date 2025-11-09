using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public class HolidayCalendarServiceImpl : HolidayCalendarService
    {
        private readonly HolidayCalendarRepository _holidayRepo;

        public HolidayCalendarServiceImpl(HolidayCalendarRepository holidayRepo)
        {
            _holidayRepo = holidayRepo;
        }

        public async Task<IEnumerable<HolidayCalendarDTO>> GetAllAsync()
        {
            var holidays = await _holidayRepo.GetAllAsync();
            return holidays.Select(ToDTO);
        }

        public async Task<HolidayCalendarDTO?> GetByIdAsync(int id)
        {
            var holiday = await _holidayRepo.GetByIdAsync(id);
            return holiday == null ? null : ToDTO(holiday);
        }

        public async Task<IEnumerable<HolidayCalendarDTO>> GetByDepartmentAsync(int depId)
        {
            var holidays = await _holidayRepo.GetByDepartmentAsync(depId);
            return holidays.Select(ToDTO);
        }

        public async Task<IEnumerable<HolidayCalendarDTO>> GetByEmployeeInRange(int empId, DateTime start, DateTime end)
        {
            var holidays = await _holidayRepo.GetByEmployeeAsync(empId, start, end);
            return holidays;
        }

        public async Task<HolidayCalendarDTO> AddAsync(HolidayCalendarDTO dto)
        {
            var entity = new HolidayCalendar
            {
                HolidayName = dto.HolidayName,
                PeriodYear = dto.PeriodYear
            };

            await _holidayRepo.AddAsync(entity);

            // cập nhật lại DTO theo dữ liệu từ DB
            dto.HolidayId = entity.HolidayId;
            dto.CreatedAt = entity.CreatedAt;
            return dto;
        }

        public async Task<HolidayCalendarDTO> UpdateAsync(HolidayCalendarDTO dto)
        {

            await _holidayRepo.UpdateAsync(dto);
            return dto;
        }

        public async Task AssignHolidayToDepartmentAsync(int holidayId, int depId, DateTime startDate, DateTime endDate)
        {
            await _holidayRepo.AssignHolidayToDepartmentAsync(holidayId, depId, startDate, endDate);
        }

        public async Task RemoveHolidayFromDepartmentAsync(int holidayId, int depId)
        {
            await _holidayRepo.RemoveHolidayFromDepartmentAsync(holidayId, depId);
        }

        public async Task<List<HolidayCalendarDTO>> GetFilteredHolidaysAsync(DateTime? start, DateTime? end, string? name)
        {
            return await _holidayRepo.GetFilteredHolidaysAsync(start, end, name);
        }

        // Helper: map entity → DTO
        private HolidayCalendarDTO ToDTO(HolidayCalendar h)
        {
            return new HolidayCalendarDTO
            {
                HolidayId = h.HolidayId,
                HolidayName = h.HolidayName,
                PeriodYear = h.PeriodYear,
                CreatedAt = h.CreatedAt
            };
        }
    }
}
