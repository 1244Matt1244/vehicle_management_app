using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Ninject;
using Project.MVC.Infrastructure;
using Ninject.Web.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Initialize Ninject kernel
var kernel = new StandardKernel(new NinjectBootstrapper());

// Use correct constructor for NinjectServiceProviderFactory
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory());

builder.Host.ConfigureContainer<IKernel>(kernel => {});

// Rest of middleware setup
var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=VehicleMake}/{action=Index}/{id?}");
app.Run();
