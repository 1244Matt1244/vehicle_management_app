using Ninject.Modules;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Repositories;
using Project.Service.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Project.Service.Mappings;
using Project.MVC.Mappings;

namespace Project.MVC.Infrastructure
{
    public class NinjectBootstrapper : NinjectModule
    {
        public override void Load()
        {
            // Database context
            Bind<ApplicationDbContext>()
                .ToSelf()
                .InSingletonScope()
                .WithConstructorArgument("options", 
                    new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlite("Data Source=vehicles.db")
                        .Options);

            // Repository
            Bind<IVehicleRepository>()
                .To<VehicleRepository>()
                .InTransientScope(); // Use transient or singleton based on your needs

            // Service
            Bind<IVehicleService>()
                .To<VehicleService>()
                .InTransientScope(); // Use transient or singleton based on your needs

            // AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
            });
            Bind<IMapper>()
                .ToConstant(config.CreateMapper())
                .InSingletonScope();
        }
    }
}
