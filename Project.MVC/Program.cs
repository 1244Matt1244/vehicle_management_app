using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Project.Service.Data.Context;
using Project.Service.Mappings;
using Project.MVC.Mappings;
using Project.Service;
using Project.Service.Services;
using Project.Service.Interfaces;
using Ninject;
using Serilog;
using System;
using Microsoft.EntityFrameworkCore; // For UseSqlServer

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure Serilog for logging from appsettings.json
        builder.Host.UseSerilog((context, services, configuration) =>
        {
            configuration.ReadFrom.Configuration(context.Configuration);
        });

        // Configure Ninject for dependency injection
        var kernel = new StandardKernel(new ServiceModule()); // Updated to ServiceModule
        builder.Services.AddSingleton<IKernel>(kernel);

        // Register services using Ninject
        builder.Services.AddScoped<IVehicleService>(provider => kernel.Get<IVehicleService>());

        // Add services to the container
        builder.Services.AddControllersWithViews();

        // Update database context configuration with SQL Server resiliency
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null // Pass null for this parameter
                    );
                }
            )
        );

        // Configure AutoMapper with profiles
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

        // Build the application
        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();

        // Apply migrations at startup
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.Migrate();
        }

        app.UseAntiforgery(); // CSRF Protection Middleware
        app.UseAuthorization();

        // Configure default route
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=VehicleMake}/{action=Index}/{id?}");

        // Run the application
        app.Run();
    }
}
