using Ninject.Modules;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Services;

namespace Project.Service
{
    public class VehicleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationDbContext>().ToSelf().InSingletonScope();
            Bind<IVehicleService>().To<VehicleService>();
        }
    }
}