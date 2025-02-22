using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ninject;
using Ninject.Modules;
using Project.MVC.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Configure Ninject
var kernel = new StandardKernel(new NinjectBootstrapper());

// Populate the Ninject kernel with services from the IServiceCollection
kernel.Populate(builder.Services);

// Register the Ninject service provider factory
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory());

// Add other services to the container
builder.Services.AddControllersWithViews(); // Example for MVC

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
