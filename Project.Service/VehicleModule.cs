using Ninject.Modules;
using Project.Service.Interfaces; 
using Project.Service.Repositories;
using Project.Service.Services;

namespace Project.Service
{
    public class VehicleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IVehicleRepository>().To<VehicleRepository>();
            Bind<IVehicleService>().To<VehicleService>();
        }
    }
}