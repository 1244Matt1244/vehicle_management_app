using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection; // Add this
using Microsoft.EntityFrameworkCore; // Add if using EF Core
using Project.Service.Data.Context; // Update with your DbContext namespace

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); // Fixes CS1061

// Install NuGet: Ninject, Ninject.Web.AspNetCore
builder.Host.UseNinject();

// In NinjectModule:
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Example DbContext registration (adjust as needed)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Middleware setup here...
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();