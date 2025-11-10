using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services.ServiceList
{
    public class ClockInAdjustmentRequestServiceImpl : ClockInAdjustmentRequestService
    {
        private readonly ClockInAdjustmentRequestRepository clockInAdjustmentRequestRepository;

        public ClockInAdjustmentRequestServiceImpl(ClockInAdjustmentRequestRepository clockInAdjustmentRequestRepository)
        {
            this.clockInAdjustmentRequestRepository = clockInAdjustmentRequestRepository;
        }

        public async Task<bool> AddClockInAdjustmentRequest(ClockinAdjustmentRequestCreateDTO dto)
        {
            if (dto == null) return false;

            string? savedFilePath = null;

            // Nếu có file đính kèm thì lưu nó
            if (dto.Attachment != null && dto.Attachment.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tạo tên file duy nhất để tránh trùng
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(dto.Attachment.FileName)}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await dto.Attachment.CopyToAsync(stream);
                }

                // Lưu đường dẫn tương đối để hiển thị/public sau này
                savedFilePath = $"/uploads/{fileName}";
            }

            // Tạo object entity để lưu DB
            var newClockInAdjustmentRequest = new ClockInAdjustmentRequest
            {
                EmployeeId = dto.EmployeeId,
                ClockInComponentId = dto.ClockInComponentId,
                RequestedValue = dto.RequestedValue,
                Message = dto.Message,
                Attachment = savedFilePath  // Lưu đường dẫn file (nếu có)
            };

            await clockInAdjustmentRequestRepository.AddAsync(newClockInAdjustmentRequest);
            return true;
        }

        public async Task<List<ClockinAdjustmentRequestGroupDTO>> GetClockinAdjustmentRequestGroupByDepID()
        {
            return await clockInAdjustmentRequestRepository.GetGroupByDepID();
        }

        public async Task<List<ClockinAdjustmentRequestGroupDTO>> GetClockinAdjustmentRequestGroupByDepIdAndDateRange(DateTime from, DateTime to)
        {
            return await clockInAdjustmentRequestRepository.GetGroupByDepIdAndDateRange(from, to);
        }

        public async Task<List<IGrouping<int?, ClockInAdjustmentRequest>>> GetGroupByDepIDTest()
        {
            return await clockInAdjustmentRequestRepository.GetGroupByDepIDTest();
        }

        public async Task<bool> RespondAsync(ClockinAdjustmentRespondDTO dto)
        {
            var req = await clockInAdjustmentRequestRepository.GetAdjustmentRequestByIdAsync(dto.RequestId);
            if (req == null) return false;

            req.Status = dto.status;
            req.Comment = dto.Comment;
            req.ApproverId = dto.ApproverId;

            await clockInAdjustmentRequestRepository.UpdateAsync(req);
            return true;
        }
    }
}
