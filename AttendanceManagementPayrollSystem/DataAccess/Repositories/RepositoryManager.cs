using AttendanceManagementPayrollSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceManagementPayrollSystem.DataAccess.Repositories
{
    public class RepositoryManager
    {
        public static void DoScoped(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContextPool<AttendanceManagementPayrollSystemContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
            );
            builder.Services.AddScoped<EmployeeRepository, EmployeeRepositoryImpl>();
            builder.Services.AddScoped<ClockinRepository, ClockinRepositoryImpl>();
            builder.Services.AddScoped<PayRunRepository, PayRunRepositoryImpl>();
            builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepositoryImpl>();
            builder.Services.AddScoped<ShiftRepository, ShiftRepositoryImpl>();
            builder.Services.AddScoped<DepartmentHolidayCalendarRepository, DepartmentHolidayCalendarRepositoryImpl>();
            builder.Services.AddScoped<HolidayCalendarRepository, HolidayCalendarRepositoryImpl>();
            builder.Services.AddScoped<DepartmentRepository, DepartmentRepositoryImpl>();
            builder.Services.AddScoped<SalaryPolicyRepository, SalaryPolicyRepositoryImpl>();
            builder.Services.AddScoped<IOvertimeRepository, OvertimeRepositoryImpl>();
            builder.Services.AddScoped<TaxRepository, TaxRepositoryImpl>();
            builder.Services.AddScoped<InsuranceRateRepository, InsuranceRateRepositoryImpl>();
            builder.Services.AddScoped<ClockInAdjustmentRequestRepository, ClockInAdjustmentRequestRepositoryImpl>();
            builder.Services.AddScoped<ClockinComponentRepository, ClockinComponentRepositoryImpl>();
            builder.Services.AddScoped<EmployeeRepository, EmployeeRepositoryImpl>();




        }

    }
}
