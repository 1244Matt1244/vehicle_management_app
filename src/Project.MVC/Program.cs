// Project.MVC/Program.cs
using Ninject.Web.AspNetCore.Hosting;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Ninject Configuration
builder.Host.UseNinject();
builder.Host.ConfigureContainer<NinjectServiceProviderBuilder>((context, services) =>
{
    // Database Context
    services.Bind<ApplicationDbContext>()
        .ToSelf()
        .InRequestScope()
        .WithConstructorArgument("options", 
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(context.Configuration.GetConnectionString("DefaultConnection"))
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
    pattern: "{controller=Make}/{action=Index}/{id?}");

app.Run();