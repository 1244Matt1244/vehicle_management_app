using Ninject;
using Ninject.Web.Common.WebHost;
using Project.MVC.Mappings;
using Project.Service.Interfaces;
using Project.Service.Services;
using System.Web;
using System.Web.Mvc;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Project.MVC.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Project.MVC.App_Start.NinjectWebCommon), "Stop")]

namespace Project.MVC.App_Start
{
    public static class NinjectWebCommon
    {
        private static IKernel _kernel;

        public static void Start()
        {
            _kernel = new StandardKernel();
            RegisterServices(_kernel);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(_kernel));
        }

        private static void RegisterServices(IKernel kernel)
        {
            // Service Layer
            kernel.Bind<IVehicleService>().To<VehicleService>();
            
            // AutoMapper Configuration
            kernel.Bind<IMapper>().ToMethod(ctx => 
                new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<ServiceMappingProfile>();
                    cfg.AddProfile<MvcMappingProfile>();
                }).CreateMapper()
            );
        }
    }

    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;
        public NinjectDependencyResolver(IKernel kernel) => _kernel = kernel;
        public object GetService(Type serviceType) => _kernel.TryGet(serviceType);
        public IEnumerable<object> GetServices(Type serviceType) => _kernel.GetAll(serviceType);
    }
}