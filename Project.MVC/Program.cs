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
using Project.MVC.Mappings;
using Project.Service.Mappings;

var builder = WebApplication.CreateBuilder(args);

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
builder.Services.AddControllersWithViews();
builder.Services.AddAuthorization();

var app = builder.Build();

// Middleware pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=VehicleMake}/{action=Index}/{id?}");

app.Run();