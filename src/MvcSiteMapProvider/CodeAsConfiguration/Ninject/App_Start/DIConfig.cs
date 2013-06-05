using System;
using Ninject;
using DI;
using DI.Ninject;
using DI.Ninject.Modules;

internal class DIConfig
{
    public static IDependencyInjectionContainer Register()
    {
        // Create the DI container
        var container = new StandardKernel();

        // Setup configuration of DI
        container.Load(new MvcModule());
        container.Load(new MvcSiteMapProviderModule());

        // Return our DI container wrapper instance
        return new NinjectContainer(container);
    }
}

