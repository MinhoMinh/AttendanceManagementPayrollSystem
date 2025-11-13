using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public class DepartmentWeeklyShiftServiceImpl : DepartmentWeeklyShiftService
    {
        private readonly DepartmentWeeklyShiftRepository _repository;

        public DepartmentWeeklyShiftServiceImpl(DepartmentWeeklyShiftRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<DepartmentWeeklyShiftViewDTO>> GetAllForViewAsync()
        {
            // có thể thêm business logic ở đây nếu cần
            return await _repository.GetAllForViewAsync();
        }

        public async Task<DepartmentWeeklyShiftViewDTO> GetByIdForViewAsync(int deptShiftId)
        {
            // kiểm tra xem tồn tại không
            var result = await _repository.GetByIdForViewAsync(deptShiftId);
            if (result == null)
            {
                // có thể ném exception hoặc return null tuỳ nhu cầu
                return null;
            }
            return result;
        }

        public async Task<bool> UpdateAsync(DepartmentWeeklyShiftUpdateDTO dto)
        {
            // có thể thêm business rule, ví dụ kiểm tra tồn tại ShiftId, DepId trước khi update
            return await _repository.UpdateAsync(dto);
        }
    }
}
