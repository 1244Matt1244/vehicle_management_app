using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Ninject.Extensions.DependencyInjection;

namespace Project.MVC.Infrastructure
{
    public class NinjectServiceProviderFactory : IServiceProviderFactory<IKernel>
    {
        public IKernel CreateBuilder(IServiceCollection services) => 
            new NinjectBootstrapper().GetKernel(services);

        public IServiceProvider CreateServiceProvider(IKernel container) => 
            container.Get<IServiceProvider>();
    }
}