using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Ninject.Web.AspNetCore; // Add this
using Ninject.Web.AspNetCore.Hosting;
using Project.MVC.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configure Ninject
var kernel = new AspNetCoreKernel(new NinjectBootstrapper()); // Use AspNetCoreKernel instead of StandardKernel
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory(kernel));

var app = builder.Build();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.MapControllerRoute(name: "default", pattern: "{controller=VehicleMake}/{action=Index}/{id?}");
app.Run();