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
            var mapperConfig = new MapperConfiguration(cfg => {
                cfg.AddProfile<ServiceMappingProfile>();
            });
            
            Bind<IMapper>().ToConstructor(c => new Mapper(mapperConfig)).InSingletonScope();

            // Database context
            Bind<ApplicationDbContext>().ToSelf().InSingletonScope()
                .WithConstructorArgument("options", new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VehicleManagement;Trusted_Connection=True;")
                    .Options);

            // Services
            Bind<IVehicleService>().To<VehicleService>();
        }
    }
}