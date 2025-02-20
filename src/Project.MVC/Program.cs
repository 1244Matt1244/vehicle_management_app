// Program.cs
using Ninject.Web.AspNetCore.Hosting;
using Project.Service.Data.Context;

var builder = WebApplication.CreateBuilder(args);

// Add global error handling
builder.Services.AddControllersWithViews(options => 
{
    options.Filters.Add<GlobalExceptionFilter>();
});

// Ninject
builder.Host.UseNinject();
builder.Host.ConfigureContainer<NinjectServiceProviderBuilder>(services =>
{
    services.Bind<ApplicationDbContext>()
        .ToSelf()
        .InRequestScope()
        .WithConstructorArgument("options", 
            new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
                .Options);

    services.Bind<IVehicleService>().To<VehicleService>();
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
    pattern: "{controller=Vehicle}/{action=Makes}/{id?}");

app.Run();