using System;
using System.Web.Mvc;
using StructureMap;
using DI;
using DI.StructureMap.Registries;

internal class DependencyResolverConfig
{
    public static IDependencyInjectionContainer Register()
    {
        // Create the DI container
        var container = new Container();

        // Setup configuration of DI
        container.Configure(r => r.AddRegistry(new StructureMapRegistry(container)));
        container.Configure(r => r.AddRegistry<MvcSiteMapProviderRegistry>());
        container.Configure(r => r.AddRegistry<MvcRegistry>());
        container.Configure(r => r.AddRegistry<MvcDependencyResolverRegistry>());

        // Verify the configuration
        // TODO: Move this into a test
        //container.AssertConfigurationIsValid();

        // Reconfigure MVC to use Service Location

        // You will need to use DependencyResolver if there are hard references
        // to IDependencyResolver in your code somewhere (don't do that - it limits your options
        // and tightly couples your code to MVC!).
        DependencyResolver.SetResolver(container.GetInstance<IDependencyResolver>());

        // Return our DI container wrapper instance
        return container.GetInstance<IDependencyInjectionContainer>();
    }
}
