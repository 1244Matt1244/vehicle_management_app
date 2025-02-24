using System.Collections.Generic;
using Ninject.Modules;
using Microsoft.EntityFrameworkCore;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Data.Helpers;
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
                .InTransientScope() // Use Transient for EF Core
                .WithConstructorArgument("options",
                    new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlite("Data Source=vehicles.db")
                        .Options);

            // Repository and service bindings
            Bind<IVehicleRepository>().To<VehicleRepository>();
            Bind<IVehicleService>().To<VehicleService>();

            // AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
                cfg.CreateMap(typeof(PaginatedList<>), typeof(PaginatedList<>))
                    .ConvertUsing(typeof(PaginatedListConverter<,>));
            });
            
            Bind<IMapper>().ToConstant(config.CreateMapper());
        }
    }
}
