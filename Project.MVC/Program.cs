using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Ninject;
using Project.Service.Mappings;
using Project.MVC.Mappings;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new NinjectServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}