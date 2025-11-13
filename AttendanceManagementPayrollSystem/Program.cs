using AttendanceManagementPayrollSystem.UI;
using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using AttendanceManagementPayrollSystem.Services.ServiceList;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


builder.Services.AddControllersWithViews();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            origin.StartsWith("http://localhost:") || origin.StartsWith("https://localhost:")
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});


// Register services
builder.Services.AddScoped<PayRunService, PayRunServiceImpl>();
builder.Services.AddScoped<KPIService, KPIServiceImpl>();
builder.Services.AddScoped<ClockinService, ClockinServiceImpl>();
builder.Services.AddScoped<AuthService, AuthServiceImpl>();
builder.Services.AddScoped<LeaveRequestService, LeaveRequestServiceImpl>();
builder.Services.AddScoped<HolidayCalendarService, HolidayCalendarServiceImpl>();
builder.Services.AddScoped<ShiftService, ShiftServiceImpl>();
builder.Services.AddScoped<HolidayCalendarService, HolidayCalendarServiceImpl>();
builder.Services.AddScoped<DepartmentService, DepartmentServiceImpl>();
builder.Services.AddScoped<IOvertimeService, OvertimeService>();
builder.Services.AddScoped<SalaryPolicyService, SalaryPolicyServiceImpl>();
builder.Services.AddScoped<TaxService, TaxServiceImpl>();
builder.Services.AddScoped<InsuranceRateService, InsuranceRateServiceImpl>();
builder.Services.AddScoped<ClockInAdjustmentRequestService, ClockInAdjustmentRequestServiceImpl>();
builder.Services.AddScoped<ClockinComponentService, ClockinComponentServiceImpl>();
builder.Services.AddScoped<EmployeeService, EmployeeServiceImpl>();
builder.Services.AddScoped<DepartmentWeeklyShiftService, DepartmentWeeklyShiftServiceImpl>();
builder.Services.AddScoped<EmployeeDependentService, EmployeeDependentServiceImpl>();







builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        o.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

// Add Repository in here
RepositoryManager.DoScoped(builder);

builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("http://localhost:5038/");
});

builder.Services
    .AddBlazorise(options =>
    {
        options.Immediate = true;
    })
.AddBootstrap5Providers()
.AddFontAwesomeIcons();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowReactApp");
app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapControllers();

app.Run();
