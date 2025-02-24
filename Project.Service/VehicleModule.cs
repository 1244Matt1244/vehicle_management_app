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
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IVehicleRepository>()
                .To<VehicleRepository>()
                .InSingletonScope();

            Bind<IVehicleService>()
                .To<VehicleService>()
                .InSingletonScope();
        }
    }
}