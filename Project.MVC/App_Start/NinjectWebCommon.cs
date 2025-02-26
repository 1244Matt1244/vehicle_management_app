using Ninject;
using Ninject.Web.Common;
using Project.Service.Interfaces;
using Project.Service.Services;
using AutoMapper;
using Project.Service.MappingProfiles; // Adjust according to your project structure
using System;
using System.Web;

namespace Project.MVC.App_Start
{
    public static class NinjectWebCommon
    {
        private static IKernel _kernel;

        public static void Start()
        {
            _kernel = new StandardKernel();
            _kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            _kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            RegisterServices(_kernel);
        }

        private static void RegisterServices(IKernel kernel)
        {
            // Service Layer
            kernel.Bind<IVehicleService>().To<VehicleService>();
            
            // AutoMapper
            kernel.Bind<IMapper>().ToMethod(ctx =>
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ServiceMappingProfile>();
                    cfg.AddProfile<MvcMappingProfile>();
                }).CreateMapper()
            );
        }
    }
}
