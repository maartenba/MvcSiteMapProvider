using System;
using System.Web.Mvc;
using StructureMap;
using DI;
using DI.StructureMap.Registries;

internal class DIConfig
{
    public static IDependencyInjectionContainer Register()
    {
        // Create the DI container
        var container = new Container();

        // Setup configuration of DI
        container.Configure(r => r.AddRegistry(new StructureMapRegistry(container)));
        container.Configure(r => r.AddRegistry<MvcSiteMapProviderRegistry>());
        container.Configure(r => r.AddRegistry<MvcControllerFactoryRegistry>());

        // Verify the configuration
        // TODO: Move this into a test
        //container.AssertConfigurationIsValid();

        // Reconfigure MVC to use StructureMap for DI
        ControllerBuilder.Current.SetControllerFactory(container.GetInstance<IControllerFactory>());

        // Return our DI container wrapper instance
        return container.GetInstance<IDependencyInjectionContainer>();
    }
}
