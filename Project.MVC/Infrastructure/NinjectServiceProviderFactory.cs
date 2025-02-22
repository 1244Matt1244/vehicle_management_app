using Ninject;
using Microsoft.Extensions.DependencyInjection;

namespace Project.MVC.Infrastructure
{
    public class NinjectServiceProviderFactory : IServiceProviderFactory<IKernel>
    {
        public IKernel CreateBuilder(IServiceCollection services)
        {
            var kernel = new StandardKernel();
            // Manually bind services here
            foreach (var service in services)
            {
                if (service.Lifetime == ServiceLifetime.Singleton)
                {
                    kernel.Bind(service.ServiceType).To(service.ImplementationType).InSingletonScope();
                }
                else if (service.Lifetime == ServiceLifetime.Scoped)
                {
                    kernel.Bind(service.ServiceType).To(service.ImplementationType).InScope(ctx => ctx.Kernel);
                }
                else if (service.Lifetime == ServiceLifetime.Transient)
                {
                    kernel.Bind(service.ServiceType).To(service.ImplementationType).InTransientScope();
                }
            }
            return kernel;
        }

        public IServiceProvider CreateServiceProvider(IKernel containerBuilder)
        {
            return new NinjectServiceProvider(containerBuilder); // Ensure this returns IServiceProvider
        }
    }

    public class NinjectServiceProvider : IServiceProvider
    {
        private readonly IKernel _kernel;

        public NinjectServiceProvider(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }
    }
}
