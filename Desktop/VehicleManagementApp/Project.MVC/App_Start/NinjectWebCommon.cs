// Project.MVC/App_Start/NinjectWebCommon.cs
using System;
using System.Web;
using AutoMapper;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using Ninject;
using Ninject.Web.Common;
using Project.Service.Data;
using Project.Service.Services;
using Project.MVC.MappingProfiles;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Project.MVC.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(Project.MVC.App_Start.NinjectWebCommon), "Stop")]

namespace Project.MVC.App_Start
{
    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }
        
        private static void RegisterServices(IKernel kernel)
        {
            // Bindings
            kernel.Bind<VehicleContext>().ToSelf().InRequestScope();
            kernel.Bind<IVehicleService>().To<VehicleService>().InRequestScope();

            // AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<VehicleMappingProfile>();
            });
            kernel.Bind<IMapper>().ToConstant(config.CreateMapper());
        }        
    }
}
