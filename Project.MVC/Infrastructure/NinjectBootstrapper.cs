using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Ninject.Extensions.DependencyInjection;
using Project.Service.Data.Context;
using Project.Service.Interfaces;
using Project.Service.Repositories;
using Project.Service.Services;
using AutoMapper;

namespace Project.MVC.Infrastructure
{
    public class NinjectBootstrapper : NinjectModule
    {
        private readonly IServiceCollection _services;

        public NinjectBootstrapper() { }
        public NinjectBootstrapper(IServiceCollection services) => _services = services;

        public IKernel GetKernel(IServiceCollection services)
        {
            var kernel = new StandardKernel();
            kernel.Load(this);
            kernel.Bind<IServiceCollection>().ToConstant(services);
            return kernel;
        }

        public override void Load()
        {
            // Database context (scoped per request)
            Bind<ApplicationDbContext>().ToSelf()
                .InSingletonScope()
                .WithConstructorArgument("options", ctx => 
                    new DbContextOptionsBuilder<ApplicationDbContext>()
                        .UseSqlite("Data Source=vehicles.db")
                        .Options);

            // Repository
            Bind<IVehicleRepository>().To<VehicleRepository>();

            // Service
            Bind<IVehicleService>().To<VehicleService>();

            // AutoMapper
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ServiceMappingProfile>();
                cfg.AddProfile<MvcMappingProfile>();
            });
            Bind<IMapper>().ToConstant(config.CreateMapper());
        }
    }
}