// Project.MVC/Infrastructure/NinjectBootstrapper.cs
using Ninject.Modules;
using Ninject.Web.Common;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Repositories;
using Project.Service.Services;
using AutoMapper;
using Project.Service.Mappings;
using Project.MVC.Mappings;

namespace Project.MVC.Infrastructure
{
    public class NinjectBootstrapper : NinjectModule
    {
        public override void Load()
        {
            // Database context (scoped per request)
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

            // AutoMapper (Service + MVC mappings)
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>(); // Maps DTOs → VM
            });
            Bind<IMapper>().ToConstant(config.CreateMapper());
        }
    }
}