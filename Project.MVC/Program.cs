// 2. Project.MVC/Program.cs (Simplified DI)
using Microsoft.EntityFrameworkCore;
using Project.MVC.Mappings;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MvcMappingProfile));
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware pipeline
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
    pattern: "{controller=VehicleMake}/{action=Index}/{id?}");

app.Run();