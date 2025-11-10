
using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.DTO;

namespace AttendanceManagementPayrollSystem.Services
{
    public class ClockinComponentServiceImpl : ClockinComponentService
    {
        private readonly ClockinComponentRepository clockinComponentRepository;

        public ClockinComponentServiceImpl(ClockinComponentRepository clockinComponentRepository)
        {
            this.clockinComponentRepository = clockinComponentRepository;
        }

        public async Task<bool> UpdateByRespond(ClockinComponentRespondDTO dto)
        {
            var req = await this.clockinComponentRepository.GetById(dto.Id);

            if (req == null) return false;

            req.OverridedWorkunits = dto.OverridedWorkunits;

            await this.clockinComponentRepository.UpdateAsync(req);
            return true;
        }
    }
}
