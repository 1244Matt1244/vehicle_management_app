using Ninject.Modules;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Services;

namespace Project.MVC
{
    public class VehicleModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ApplicationDbContext>().ToSelf().InSingletonScope();
            Bind<IVehicleService>().To<VehicleService>();
            
            // Add other bindings as needed
            Bind<IMapper>().ToMethod(ctx => 
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<Project.Service.Mapping.ServiceMappingProfile>();
                    cfg.AddProfile<Project.MVC.Mapping.MvcMappingProfile>();
                }).CreateMapper()
            );
        }
    }
}