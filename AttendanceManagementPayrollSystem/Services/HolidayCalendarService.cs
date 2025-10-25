using AttendanceManagementPayrollSystem.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface HolidayCalendarService
    {
        Task<IEnumerable<HolidayCalendarDTO>> GetAllAsync();
        Task<HolidayCalendarDTO?> GetByIdAsync(int id);
        Task<IEnumerable<HolidayCalendarDTO>> GetByDepartmentAsync(int depId);
        Task<HolidayCalendarDTO> AddAsync(HolidayCalendarDTO dto);
        Task<HolidayCalendarDTO> UpdateAsync(HolidayCalendarDTO dto);
        Task AssignHolidayToDepartmentAsync(int holidayId, int depId, DateTime startDate, DateTime endDate);
        Task RemoveHolidayFromDepartmentAsync(int holidayId, int depId);
    }
}
