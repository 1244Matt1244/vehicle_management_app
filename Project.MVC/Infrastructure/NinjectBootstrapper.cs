using Ninject.Modules;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Repositories;
using Project.Service.Services;
using Project.Service.Mappings;
using Project.MVC.Mappings; // Add this namespace

namespace Project.MVC.Infrastructure
{
    public class NinjectBootstrapper : NinjectModule
    {
        public override void Load()
        {
            // Database context (using InSingletonScope for .NET Core compatibility)
            Bind<ApplicationDbContext>().ToSelf().InSingletonScope()
                .WithConstructorArgument("options", 
                    new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlite("Data Source=vehicles.db")
                        .Options);

            // Repository and service
            Bind<IVehicleRepository>().To<VehicleRepository>();
            Bind<IVehicleService>().To<VehicleService>();

            // AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>(); // Now resolves
            });
            Bind<IMapper>().ToConstant(config.CreateMapper());
        }
    }
}