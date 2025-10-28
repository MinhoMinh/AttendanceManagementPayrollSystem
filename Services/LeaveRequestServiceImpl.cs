// Implementation
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTOs;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public class LeaveRequestServiceImpl : LeaveRequestService
    {
        private readonly ILeaveRequestRepository _repository;

        public LeaveRequestServiceImpl(ILeaveRequestRepository repository)
        {
            _repository = repository;
        }

        public async Task<LeaveRequestDTO> AddAsync(LeaveRequestDTO dto)
        {
            var totalDays = (dto.EndDate.ToDateTime(TimeOnly.MinValue) - dto.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays + 1;

            var entity = new LeaveRequest
            {
                EmpId = dto.EmpId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                Status = "Pending",
                NumbersOfDays = (decimal)totalDays,
                TypeId = dto.TypeId
            };

            await _repository.AddAsync(entity);
            return dto;
        }

        public async Task<IEnumerable<LeaveRequestDTO>> GetByEmployeeIdAsync(int empId)
        {
            var list = await _repository.GetByEmployeeIdAsync(empId);
            return list.Select(lr => new LeaveRequestDTO
            {
                ReqId = lr.ReqId,
                EmpId = lr.EmpId,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                Reason = lr.Reason,
                TypeId = lr.TypeId,
                Status = lr.Status
            }).ToList();
        }

        public async Task<IEnumerable<LeaveRequestDTO>> GetPendingAsync()
        {
            var list = await _repository.GetPendingAsync();
            return list.Select(lr => new LeaveRequestDTO
            {
                ReqId = lr.ReqId,
                EmpId = lr.EmpId,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                Reason = lr.Reason,
                TypeId = lr.TypeId,
                Status = lr.Status
            }).ToList();
        }

        public async Task UpdateStatusAsync(LeaveRequestStatusDTO dto)
        {
            var request = await _repository.GetByIdAsync(dto.ReqId);
            if (request == null) throw new Exception("Leave request not found");

            request.Status = dto.Status;

            await _repository.UpdateAsync(request);
        }
    }
}
