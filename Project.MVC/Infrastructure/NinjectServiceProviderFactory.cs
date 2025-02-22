using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Ninject.Extensions.DependencyInjection;

namespace Project.MVC.Infrastructure
{
    public class NinjectServiceProviderFactory : IServiceProviderFactory<IKernel>
    {
        private readonly NinjectSettings _settings;
        
        public NinjectServiceProviderFactory() : this(new NinjectSettings()) { }
        
        public NinjectServiceProviderFactory(NinjectSettings settings)
        {
            _settings = settings;
        }

        public IKernel CreateBuilder(IServiceCollection services)
        {
            var kernel = new StandardKernel(_settings);
            kernel.Populate(services);
            return kernel;
        }

        public IServiceProvider CreateServiceProvider(IKernel container)
        {
            return container.Get<IServiceProvider>();
        }
    }
}