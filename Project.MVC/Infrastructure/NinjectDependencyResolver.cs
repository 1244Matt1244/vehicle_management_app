using Ninject;
using Ninject.Modules;
using Project.Service.Data;
using Project.Service.Interfaces;
using Project.Service.Services;
using Microsoft.EntityFrameworkCore;

namespace Project.MVC.Infrastructure
{
    public class NinjectDependencyResolver : NinjectModule
    {
        public override void Load()
        {
            // Database context
            Bind<ApplicationDbContext>()
                .ToSelf()
                .InTransientScope()
                .WithConstructorArgument("options", 
                    new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlite("Data Source=vehicles.db")
                        .Options);

            // Repository
            Bind<IVehicleRepository>()
                .To<VehicleRepository>()
                .InSingletonScope();

            // Service
            Bind<IVehicleService>()
                .To<VehicleService>()
                .InSingletonScope();

            // Automapper
            Bind<IMapper>().ToMethod(ctx =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<Project.Service.Mappings.ServiceMappingProfile>();
                    cfg.AddProfile<Project.MVC.Mappings.MvcMappingProfile>();
                });
                return config.CreateMapper();
            }).InSingletonScope();
        }
    }
}