using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Project.Service.Interfaces;
using Project.Service.Services;
using Project.Service.Data.Context;
using AutoMapper;
using Project.MVC.Helpers;
using Project.Service.Mappings;
using Project.MVC.Mappings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Database Context
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper Configuration
builder.Services.AddAutoMapper(
    typeof(ServiceMappingProfile),        // Service Layer Mappings
    typeof(MvcMappingProfile)      // MVC Mappings
);

// Service Registration
builder.Services.AddScoped<IVehicleService, VehicleService>();

var app = builder.Build();

// Middleware pipeline
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
