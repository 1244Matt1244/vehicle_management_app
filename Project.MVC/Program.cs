using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ninject;
using Ninject.Web.AspNetCore.Hosting;
using Project.Service.Data;
using Project.Service.Data.Helpers;
using Project.Service.Interfaces;
using Project.Service.Services;
using Project.Service.Mappings;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Configure Ninject
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory())
    .ConfigureContainer<IKernel>(kernel =>
    {
        // Database Configuration
        kernel.Bind<ApplicationDbContext>()
            .ToSelf()
            .InTransientScope()
            .WithConstructorArgument("options", 
                new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlite("Data Source=vehicles.db")
                    .Options);

        // Service Layer
        kernel.Bind<IVehicleService>()
            .To<VehicleService>()
            .InSingletonScope();

        // AutoMapper Configuration
        kernel.Bind<IMapper>().ToMethod(ctx =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
                cfg.CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
                .ConvertUsing(typeof(PaginatedListConverter<,>));
            });
            return config.CreateMapper();
        }).InSingletonScope();
    });

// Add framework services
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VehicleMake}/{action=Index}/{id?}");

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();