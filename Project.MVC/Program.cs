using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ninject;
using Ninject.AspNetCore.AspNetCore;
using Project.MVC.Infrastructure;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

// Configure Ninject
var kernel = new StandardKernel();
kernel.Load(new NinjectDependencyResolver());

// Use Ninject as service provider
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory(kernel));

// Add MVC services
builder.Services.AddControllersWithViews();

// Add AutoMapper with profiles
builder.Services.AddAutoMapper(typeof(ServiceMappingProfile));

// If using separate projects for mappings
builder.Services.AddAutoMapper(
    typeof(ServiceMappingProfile),
    typeof(MvcMappingProfile) // Replace with actual profile names if needed
);

var app = builder.Build();

// Configure middleware
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();
