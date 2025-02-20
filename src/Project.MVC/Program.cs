// 1. Program.cs (Updated Ninject Configuration)
using Microsoft.EntityFrameworkCore;
using Ninject.Web.AspNetCore;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Ninject Configuration
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory());
builder.Host.ConfigureContainer<NinjectServiceProviderBuilder>(services =>
{
    // Database Context
    services.Bind<ApplicationDbContext>()
        .ToSelf()
        .InRequestScope()
        .WithConstructorArgument("options",
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
                .Options);

    // Services
    services.Bind<IVehicleService>().To<VehicleService>().InTransientScope();
});

// AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

var app = builder.Build();

// Database Migration
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}

// MVC Configuration
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vehicle}/{action=Index}/{id?}");

app.Run();