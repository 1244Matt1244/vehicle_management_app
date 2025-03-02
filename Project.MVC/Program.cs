using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Project.Service.Data.Context;
using Project.Service.Mappings;
using Project.MVC.Mappings;
using Project.Service;
using Project.Service.Services;
using Project.Service.Interfaces;
using System;
using Ninject;

var builder = WebApplication.CreateBuilder(args);

// Configure Ninject with the VehicleModule
var kernel = new StandardKernel(new VehicleModule());
builder.Services.AddSingleton<IKernel>(kernel);

// Register services using Ninject
builder.Services.AddScoped<IVehicleService>(provider => kernel.Get<IVehicleService>());

// Logging Configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var env = builder.Environment;
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();

    // In-memory database for testing and development environments
    if (env.IsEnvironment("Test") || env.IsDevelopment())
    {
        options.UseInMemoryDatabase("VehicleManagementDB");
    }
    else
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException("Database connection string is missing.");
        }
        options.UseSqlServer(connectionString);
    }
});

// AutoMapper Configuration with Profiles
builder.Services.AddAutoMapper(config =>
{
    config.AddProfile<ServiceMappingProfile>();
    config.AddProfile<MvcMappingProfile>();
});

// Enable Anti-Forgery Protection
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-CSRF-TOKEN";
});

// MVC Configuration
builder.Services.AddControllersWithViews();
builder.Services.AddRouting(options => options.LowercaseUrls = true);

var app = builder.Build();

// Middleware Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAntiforgery(); // CSRF Protection Middleware
app.UseAuthorization(); 

// Default Route Configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VehicleMake}/{action=Index}/{id?}");

app.Run();

// Required for integration testing
public partial class Program {}
