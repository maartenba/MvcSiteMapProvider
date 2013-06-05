using System;
using Castle.Windsor;
using DI;
using DI.Windsor;
using DI.Windsor.Installers;

internal class DIConfig
{
    public static IDependencyInjectionContainer Register()
    {
        // Create the DI container
        var container = new WindsorContainer();

        // Setup configuration of DI
        container.Install(new MvcSiteMapProviderInstaller());

        // Return our DI container wrapper instance
        return new WindsorDependencyInjectionContainer(container);
    }
}
