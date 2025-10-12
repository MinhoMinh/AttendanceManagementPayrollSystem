using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.DTOs;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.DataAccess.Repositories;

namespace AttendanceManagementPayrollSystem.Services
{
    public class LeaveRequestServiceImpl : ILeaveRequestService
    {
        private readonly ILeaveRequestRepository _repository;

        public LeaveRequestServiceImpl(ILeaveRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<LeaveRequestDTO> AddAsync(LeaveRequestDTO dto)
        {
            var entity = new LeaveRequest
            {
                EmpId = 3, // gắn từ localStorage của Frontend
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                Status = "Pending",
                TypeId = 1, // bạn có thể fix cứng tạm thời hoặc map theo dto.LeaveType
            };

            await _repository.AddAsync(entity);
            return dto;
        }
    }
}


