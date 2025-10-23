using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;
using AttendanceManagementPayrollSystem.Models;

namespace AttendanceManagementPayrollSystem.Services
{
    public class PayRunItemServiceImpl:PayRunItemService
    {

        private readonly PayRunItemRepository payRunItemRepository;

        public PayRunItemServiceImpl(PayRunItemRepository payRunItemRepository)
        {
            this.payRunItemRepository = payRunItemRepository;
        }
        public async Task<List<PayRunItemDTO>> GetPayRunItemsByEmpIdAsync(int empId)
        {
            return await payRunItemRepository.GetPayRunItemsByEmpIdAsync(8);
        }
    }
}
