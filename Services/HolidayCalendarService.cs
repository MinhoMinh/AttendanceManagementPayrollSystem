using AttendanceManagementPayrollSystem.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AttendanceManagementPayrollSystem.Services
{
    public interface HolidayCalendarService
    {
        Task<IEnumerable<HolidayCalendarDTO>> GetAllAsync();
        Task<HolidayCalendarDTO?> GetByIdAsync(int id);
        Task<IEnumerable<HolidayCalendarDTO>> GetByMonthAsync(int month, int year);
        Task<IEnumerable<HolidayCalendarDTO>> GetByDepartmentAsync(int depId);
        Task<HolidayCalendarDTO> AddAsync(HolidayCalendarDTO dto);
        Task<HolidayCalendarDTO> UpdateAsync(HolidayCalendarDTO dto);
        Task DeleteAsync(int id);
        Task AssignHolidayToDepartmentAsync(int holidayId, int depId);
        Task RemoveHolidayFromDepartmentAsync(int holidayId, int depId);
    }
}
