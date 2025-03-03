// Project.Service/VehicleModule.cs
using Ninject.Modules;
using AutoMapper;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Services;
using Project.Service.Mappings; // Only service-layer mappings

public class VehicleModule : NinjectModule
{
    public override void Load()
    {
        Bind<ApplicationDbContext>().ToSelf().InTransientScope();
        Bind<IVehicleService>().To<VehicleService>();
        
        // Service-layer mappings only
        var config = new MapperConfiguration(cfg => {
            cfg.AddProfile<ServiceMappingProfile>();
        });
        
        Bind<IMapper>().ToMethod(ctx => config.CreateMapper());
    }
}