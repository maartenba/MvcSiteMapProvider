using System;
using System.Web.Mvc;
using StructureMap;
using DI;
using DI.StructureMap;
using DI.StructureMap.Registries;

internal class DIConfig
{
    public static IDependencyInjectionContainer Register()
    {
        // Create the DI container
        var container = new Container();

        // Setup configuration of DI
        container.Configure(r => r.AddRegistry<MvcSiteMapProviderRegistry>());
        container.Configure(r => r.AddRegistry<MvcRegistry>());

        // Return our DI container wrapper instance
        return new StructureMapContainer(container);
    }
}