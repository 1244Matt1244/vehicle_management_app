using AutoMapper;
using Ninject.Modules;
using Project.Service.Mappings;
using Project.Service.Services;
using Project.Service.Interfaces;
using Project.Service.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Project.Service
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            // AutoMapper configuration
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                // Add AutoMapper profile configurations
                cfg.AddProfile<ServiceMappingProfile>();
            });

            // Bind AutoMapper to the IMapper interface using Singleton scope
            Bind<IMapper>().ToConstructor(c => new Mapper(mapperConfig)).InSingletonScope();

            // Configure and bind the database context (ApplicationDbContext) with transient scope
            Bind<ApplicationDbContext>().ToSelf().InTransientScope()
                .WithConstructorArgument("options", new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VehicleManagement;Trusted_Connection=True;")
                    .Options);

            // Bind IVehicleService to VehicleService (Transient Scope)
            Bind<IVehicleService>().To<VehicleService>().InTransientScope();

            // Add additional service bindings as required
            // Bind<IOtherService>().To<OtherService>().InTransientScope();
        }
    }
}
