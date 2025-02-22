// Project.MVC/Program.cs
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Ninject;
using Project.MVC.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Ninject setup
var kernel = new StandardKernel(new NinjectBootstrapper());
builder.Host.UseServiceProviderFactory(new NinjectServiceProviderFactory(kernel));

var app = builder.Build();

// Configure HTTP pipeline
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
    pattern: "{controller=VehicleMake}/{action=Index}/{id?}");

app.Run();