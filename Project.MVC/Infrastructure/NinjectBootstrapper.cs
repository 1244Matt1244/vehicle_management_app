using Ninject.Modules;
using Ninject.Web.Common;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Repositories;
using Project.Service.Services;
using AutoMapper;
using Project.Service.Mapping;
using Project.MVC.Mappings;

namespace Project.MVC.Infrastructure
{
    public class NinjectBootstrapper : NinjectModule
    {
        public override void Load()
        {
            // Database context (per request)
            Bind<ApplicationDbContext>()
                .ToSelf()
                .InRequestScope()
                .WithConstructorArgument("options",
                    new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlite("Data Source=vehicles.db")
                        .Options);

            // Repository and service
            Bind<IVehicleRepository>().To<VehicleRepository>();
            Bind<IVehicleService>().To<VehicleService>();

            // AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>(); // Service layer mappings
                cfg.AddProfile<MvcMappingProfile>();     // MVC ViewModel mappings
            });
            Bind<IMapper>().ToConstant(config.CreateMapper());
        }
    }
}