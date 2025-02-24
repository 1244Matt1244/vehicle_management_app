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
            Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            Bind<IVehicleRepository>().To<VehicleRepository>();
            Bind<IVehicleService>().To<VehicleService>();

            var config = new MapperConfiguration(cfg => 
                cfg.AddProfile<ServiceMappingProfile>());
            
            Bind<IMapper>().ToConstant(config.CreateMapper()).InSingletonScope();
        }
    }
}