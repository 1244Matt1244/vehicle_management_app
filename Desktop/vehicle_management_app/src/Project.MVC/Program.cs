using Microsoft.EntityFrameworkCore;
using Ninject;
using Project.Service.Data.Context;
using Project.Service.Mapping;
using Project.Service.Services;
using Project.MVC.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<CustomExceptionFilter>();
});

// Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=vehicle_management_dba.db"));

// Configure AutoMapper
builder.Services.AddAutoMapper(typeof(ServiceMappingProfile));

// Configure Dependency Injection
builder.Services.AddScoped<IVehicleService, VehicleService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();