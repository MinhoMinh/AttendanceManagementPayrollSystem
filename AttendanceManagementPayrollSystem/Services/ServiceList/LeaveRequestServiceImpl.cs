using System.Threading.Tasks;
using AttendanceManagementPayrollSystem.DTOs;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Services.Mapper;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
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

            //var totalDays = (dto.EndDate.ToDateTime(TimeOnly.MinValue) - dto.StartDate.ToDateTime(TimeOnly.MinValue)).TotalDays + 1;

            var entity = new LeaveRequest
            {

                EmpId = 3,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Reason = dto.Reason,
                Status = "Pending",
                NumbersOfDays = dto.NumbersOfDays,
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
                TypeId = lr.TypeId,
                ReqDate = lr.ReqDate,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                NumbersOfDays = lr.NumbersOfDays,
                Reason = lr.Reason,
                Status = lr.Status,
                ApprovedBy = lr.ApprovedBy,
                ApprovedDate = lr.ApprovedDate,
                ApprovedByName = lr.ApprovedByNavigation.EmpName
            }).ToList();
        }

        public async Task<IEnumerable<LeaveRequestDTO>> GetLeaveHistoryByEmployee(int empId, DateOnly? startDate, DateOnly? endDate)
        {
            var data = await _repository.GetLeaveByEmployeeId(empId, startDate, endDate);

            return data.Select(lr => new LeaveRequestDTO
            {
                ReqId = lr.ReqId,
                EmpId = lr.EmpId,
                StartDate = lr.StartDate,
                EndDate = lr.EndDate,
                NumbersOfDays = lr.NumbersOfDays,
                Reason = lr.Reason,
                TypeId = lr.TypeId,
                Status = lr.Status
            }).ToList();
        }

        public IEnumerable<LeaveType> GetRates()
        {
            var data = _repository.GetRates();
                    //.Select(x => new OvertimeRateDTO
                    //{
                    //    Id = x.Id,
                    //    OvertimeType = x.OvertimeType,
                    //    RateMultiplier = x.RateMultiplier,
                    //    EffectiveDate = x.EffectiveDate,
                    //    CreatedBy = x.CreatedBy
                    //})
                    //.ToList();

                return data;
        }
    }
}


