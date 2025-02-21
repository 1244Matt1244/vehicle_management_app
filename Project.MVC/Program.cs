using Microsoft.EntityFrameworkCore;
using Ninject.Web.AspNetCore;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Services;

var builder = WebApplication.CreateBuilder(args);

// DbContext Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Ninject Configuration
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory());
builder.Host.ConfigureContainer<NinjectServiceProviderBuilder>(services =>
{
    services.Bind<ApplicationDbContext>()
        .ToSelf()
        .InRequestScope()
        .WithConstructorArgument("options",
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
                .Options);

    services.Bind<IVehicleService>().To<VehicleService>().InTransientScope();
});

// Add Controllers and Views
builder.Host.UseNinject()
    .ConfigureServices(services =>
    {
        services.AddControllersWithViews(options => {
            options.Filters.Add<CustomExceptionFilter>();
        });
    })
    .ConfigureContainer<StandardKernel>(kernel => 
    {
        kernel.Load<VehicleModule>();
    });

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(ServiceMappingProfile), typeof(MvcMappingProfile));

var app = builder.Build();

// Database Migration
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vehicle}/{action=Index}/{id?}");

app.Run();
