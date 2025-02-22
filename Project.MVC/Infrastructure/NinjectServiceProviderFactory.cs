using Microsoft.Extensions.DependencyInjection;
using Ninject;
using Ninject.Syntax;
using Ninject.Web.Common;
using Ninject.Web.AspNetCore;
using System;

public class NinjectServiceProviderFactory : IServiceProviderFactory<IKernel>
{
    public IKernel CreateBuilder(IServiceCollection services)
    {
        var kernel = new StandardKernel();
        kernel.Bind<IServiceCollection>().ToConstant(services);
        return kernel;
    }

    public IServiceProvider CreateServiceProvider(IKernel containerBuilder)
    {
        return new NinjectServiceProvider(containerBuilder);
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
