using AutoMapper;
using AutoMapper.QueryableExtensions;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using Ninject.Web.Common.WebHost;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Mapping;
using Project.Service.Services;


namespace Project.Service
{
    public class VehicleModule : NinjectModule
    {
        public override void Load()
        {
            // Custom scope definition
            Func<IContext, object> requestScope = ctx => ctx.Kernel.Components.Get<Ninject.Activation.Caching.ICache>().TryGet(ctx.Request.ParentRequest, out var context)
                ? context : ctx;

            // Bind ApplicationDbContext
            Bind<ApplicationDbContext>()
                .ToSelf()
                .InScope(requestScope);

            // Bind VehicleService
            Bind<IVehicleService>()
                .To<VehicleService>()
                .InScope(requestScope);

            // Configure AutoMapper
            Bind<IMapper>().ToMethod(context =>
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ServiceMappingProfile>();
                });
                return config.CreateMapper();
            }).InSingletonScope();
        }
    }
}
