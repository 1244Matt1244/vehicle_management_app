using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Project.Service.Data.Context;
using Project.Service.Mappings;
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

        // Configure Ninject as the primary container
        var kernel = new StandardKernel(new VehicleModule());
        builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory(kernel));

        // Auto-register services from Ninject
        builder.Services.AddControllersWithViews()
            .AddApplicationPart(typeof(VehicleModule).Assembly);

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
