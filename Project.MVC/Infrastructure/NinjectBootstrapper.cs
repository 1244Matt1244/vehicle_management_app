using Ninject.Modules;
using AutoMapper;
using Project.Service.Mappings;
using Project.MVC.Mappings;

namespace Project.MVC.Infrastructure
{
    public class NinjectBootstrapper : NinjectModule
    {
        public override void Load()
        {
            // AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>(); // Service layer mappings
                cfg.AddProfile<MvcMappingProfile>();     // MVC view model mappings
            });
            Bind<IMapper>().ToConstant(config.CreateMapper());
        }
    }
}