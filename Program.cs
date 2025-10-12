using AttendanceManagementPayrollSystem.DataAccess.Repositories;
using AttendanceManagementPayrollSystem.Models;
using AttendanceManagementPayrollSystem.Services;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// 🔹 Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.SetIsOriginAllowed(origin =>
            origin.StartsWith("http://localhost:")
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

//builder.Services.AddDbContextPool<AttendanceManagementPayrollSystemContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
//);


builder.Services.AddScoped<PayrollService, PayrollServiceImpl>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });

builder.Services.AddScoped<EmployeeRepository, EmployeeRepositoryImpl>(); // Đăng ký EmployeeRepositoryImpl
builder.Services.AddScoped<AuthService, AuthServiceImpl>(); // Đăng ký AuthServiceImpl
builder.Services.AddScoped<ILeaveRequestRepository, LeaveRequestRepositoryImpl>();
builder.Services.AddScoped<ILeaveRequestService, LeaveRequestServiceImpl>();


RepositoryManager.DoScoped(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}







app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowReactApp");

app.UseAuthorization();



app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//app.MapGet("/hello", () =>
//{
//    return new { message = "Hello from .NET API" };
//});

app.Run();
