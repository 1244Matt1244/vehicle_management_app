// Program.cs
using AutoMapper.Extensions.Microsoft.DependencyInjection; // Ensure this line exists
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Ninject.Web.AspNetCore.Hosting;
using Project.MVC.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configure AutoMapper
builder.Services.AddAutoMapper(
    typeof(ServiceMappingProfile),
    typeof(MvcMappingProfile)
);

// Configure Ninject
var kernel = new StandardKernel(new NinjectBootstrapper());
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory(kernel));

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=VehicleMake}/{action=Index}/{id?}");
app.Run();