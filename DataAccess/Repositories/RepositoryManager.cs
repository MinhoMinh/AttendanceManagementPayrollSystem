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
            builder.Services.AddScoped<PayrollRepository, PayrollRepositoryImpl>();
        }

    }
}
