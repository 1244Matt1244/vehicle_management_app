using Ninject;
using Ninject.Modules;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Services;

namespace Project.MVC.Infrastructure
{
    public class NinjectDependencyResolver : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationDbContext>()
                .ToSelf()
                .InTransientScope();

            Bind<IVehicleService>()
                .To<VehicleService>()
                .InSingletonScope();
        }
    }
}