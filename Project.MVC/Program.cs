using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Project.Service.Interfaces;
using Project.Service.Services;
using Project.Service.Data.Context;
using AutoMapper;
using Ninject;
using Ninject.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Ninject Configuration
var kernel = new StandardKernel();
kernel.Load<VehicleModule>();
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory(kernel));

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(
    typeof(ServiceMappingProfile),
    typeof(MvcMappingProfile)
);

// Services
builder.Services.AddScoped<IVehicleService, VehicleService>();

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware
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

app.Run();