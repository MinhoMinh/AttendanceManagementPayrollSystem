using System;
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
        //private readonly IEmployeeBalanceRepository _balanceRepository;s

        public LeaveRequestServiceImpl(ILeaveRequestRepository repository)//IEmployeeBalanceRepository balanceRepository)
        {
            _repository = repository;
            //_balanceRepository = balanceRepository;
        }

        // cái này là thêm đơn xin nghỉ
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
        // lấy danh sách đơn xin nghỉ của bro nào đó
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
                TypeName = lr.Type?.Name ?? string.Empty,
                Status = lr.Status
            }).ToList();
        }

        // lấy tất cả đơn lười làm của tất cả bro
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

        // cập nhật trạng thái đơn xin nghỉ
        public async Task UpdateStatusAsync(LeaveRequestStatusDTO dto)
        {
            var request = await _repository.GetByIdAsync(dto.ReqId);
            if (request == null) throw new Exception("Leave request not found");

            request.Status = dto.Status;
            if (dto.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase) ||
                dto.Status.Equals("Rejected", StringComparison.OrdinalIgnoreCase))
            {
                request.ApprovedBy = dto.ApprovedBy;
                request.ApprovedDate = DateOnly.FromDateTime(DateTime.Now);
            }

            await _repository.UpdateAsync(request);

            //// chỉ xử lý khi duyệt đơn
            //if (dto.Status.Equals("Approved", StringComparison.OrdinalIgnoreCase))
            //{
            //    var balance = await _balanceRepository.GetByEmployeeIdAsync(request.EmpId);
            //    if (balance == null)
            //        throw new Exception("Employee balance not found");

            //    decimal days = request.NumbersOfDays;

            //    // trừ vào loại tương ứng
            //    switch (request.Type?.Name?.ToLower())
            //    {
            //        case "pto":
            //            balance.PtoUsed += days;
            //            balance.PtoAvailable = balance.PtoTotal - balance.PtoUsed;
            //            break;

            //        case "sick":
            //            balance.SickUsed += days;
            //            balance.SickAvailable = balance.SickTotal - balance.SickUsed;
            //            break;

            //        case "vacation":
            //            balance.VacationUsed += days;
            //            balance.VacationAvailable = balance.VacationTotal - balance.VacationUsed;
            //            break;

            //        case "overtime":
            //            balance.OvertimeUsed += days;
            //            balance.OvertimeAvailable = balance.OvertimeTotal - balance.OvertimeUsed;
            //            break;

            //        default:
            //            throw new Exception("Unknown leave type");
            //    }

            //    balance.LastUpdated = DateTime.Now;
            //    await _balanceRepository.UpdateAsync(balance);
            //}
        }
    }
}
