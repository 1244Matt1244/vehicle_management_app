using Ninject.Modules;
using Project.Service.Interfaces;
using Project.Service.Services;

namespace Project.Service
{
    public class VehicleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IVehicleService>().To<VehicleService>();
        }
    }
}
