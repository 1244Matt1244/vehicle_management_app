using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Ninject;
using Ninject.Web.AspNetCore.Hosting;
using Project.MVC.Mappings;
using Project.Service.Mappings;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Initialize Ninject kernel
var kernel = new StandardKernel(new NinjectBootstrapper());

// Register AutoMapper profiles
kernel.Bind<IMapper>().ToMethod(ctx => new MapperConfiguration(cfg =>
{
    // Register all profiles (you can add more profiles here if needed)
    cfg.AddProfile(new MvcMappingProfile());
    cfg.AddProfile(new ServiceMappingProfile());
}).CreateMapper());

// Use correct constructor for NinjectServiceProviderFactory
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory());

builder.Host.ConfigureContainer<IKernel>(kernel => { });

// Rest of middleware setup
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=VehicleMake}/{action=Index}/{id?}");
app.Run();
