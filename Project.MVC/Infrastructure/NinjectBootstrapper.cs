using Ninject.Modules;
using Ninject.Web.AspNetCore;
using Ninject.Web.Common;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Repositories;
using Project.Service.Services;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Project.Service.Mappings;
using Project.MVC.Mappings;
using System;

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
            Bind<IVehicleRepository>().To<VehicleRepository>();

            // Service
            Bind<IVehicleService>().To<VehicleService>();

            // AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
            });
            Bind<IMapper>().ToConstant(config.CreateMapper());
        }
    }
}