// In NinjectDependencyResolver.cs
using Project.Service.Data; // Changed from Project.Service.Data.Context
using Project.Service.Interfaces;
using Project.Service.Services;
using Ninject;
using Ninject.Modules;

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