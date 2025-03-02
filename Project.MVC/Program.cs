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
using Project.Service.Services;
using Project.Service.Interfaces;
using System.Runtime.CompilerServices;
using System;

[assembly: InternalsVisibleTo("Project.Tests")]

var builder = WebApplication.CreateBuilder(args);

// Logging Configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

// Database Configuration (Ensuring Only One Provider)
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

// Service Layer Configuration
builder.Services.AddScoped<IVehicleService, VehicleService>();

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
app.UseAuthorization(); // Ensure Authorization is applied

// Default Route Configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VehicleMake}/{action=Index}/{id?}");

app.Run();

// Required for integration testing
public partial class Program {}
