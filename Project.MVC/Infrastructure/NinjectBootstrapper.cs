using AutoMapper;
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
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
            });

            // Bind IMapper to the configured mapper instance
            Kernel.Bind<IMapper>().ToConstant(config.CreateMapper());
        }
    }
}
