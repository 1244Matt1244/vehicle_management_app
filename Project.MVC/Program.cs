using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;
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
        var kernel = new StandardKernel(new VehicleModule());
        builder.Services.AddSingleton<IKernel>(kernel);

        // Register services using Ninject
        builder.Services.AddScoped<IVehicleService>(provider => kernel.Get<IVehicleService>());

        // Add services to the container
        builder.Services.AddControllersWithViews();

        // Configure database context with environment-specific settings
        builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
        {
            var env = builder.Environment;
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            if (env.IsDevelopment())
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
            else
            {
                options.UseInMemoryDatabase("VehicleManagementDB");
            }
        });

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
