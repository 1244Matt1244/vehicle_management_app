using Ninject.Modules;
using Ninject.Web.Common;
using AutoMapper;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Repositories;
using Project.Service.Services;
using Project.Service.Mappings;

namespace Project.Service
{
    public class VehicleModule : NinjectModule
    {
        public override void Load()
        {
            // Database context (1 per request)
            Bind<ApplicationDbContext>().ToSelf().InRequestScope();

            // Repository pattern
            Bind<IVehicleRepository>().To<VehicleRepository>();

            // Service layer
            Bind<IVehicleService>().To<VehicleService>();

            // AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
            });
            
            Bind<IMapper>().ToConstant(config.CreateMapper())
                .InSingletonScope();
        }
    }
}