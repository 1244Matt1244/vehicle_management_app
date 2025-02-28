using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting; 
using Microsoft.Extensions.Configuration;
using Project.Service.Data.Context;
using Project.Service.Mappings;
using Project.MVC.Mappings;
using Project.Service.Services;
using Project.Service.Interfaces;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Project.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
if (builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => 
        options.UseInMemoryDatabase("TestDatabase"));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

// AutoMapper with Profiles
builder.Services.AddAutoMapper(config => 
{
    config.AddProfile<ServiceMappingProfile>();
    config.AddProfile<MvcMappingProfile>();
});

// Service Layer
builder.Services.AddScoped<IVehicleService, VehicleService>();

// MVC Configuration
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Middleware Pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Default Route Configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VehicleMake}/{action=Index}/{id?}");

app.Run();

public partial class Program {} // Required for integration testing
