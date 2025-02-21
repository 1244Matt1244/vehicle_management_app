using Ninject;
using Ninject.Modules;
using Ninject.Web.AspNetCore;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Services;


namespace Project.MVC.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NinjectDependencyResolver()
        {
            _kernel = new StandardKernel();
            LoadModules();
        }

        private void LoadModules()
        {
            _kernel.Bind<ApplicationDbContext>().ToSelf().InRequestScope();
            _kernel.Bind<IVehicleService>().To<VehicleService>();
            
            // AutoMapper Configuration
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<Project.Service.Mapping.ServiceMappingProfile>();
                cfg.AddProfile<Project.MVC.Mapping.MvcMappingProfile>();
            });
            _kernel.Bind<IMapper>().ToConstant(mapperConfig.CreateMapper());
        }

        public object GetService(Type serviceType) => _kernel.TryGet(serviceType);
        public IEnumerable<object> GetServices(Type serviceType) => _kernel.GetAll(serviceType);
    }
}