using AutoMapper;
using Ninject;
using Ninject.Modules;
using Project.MVC.Mappings;
using Project.Service.Mappings;

namespace Project.MVC.Infrastructure
{
    public class NinjectBootstrapper : NinjectModule
    {
        public override void Load()
        {
            // AutoMapper configuration
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
            });
            
            Kernel.Bind<IMapper>().ToConstant(mapperConfig.CreateMapper());
        }
    }
}