using System;
using System.Web.Mvc;
using DI;
using DI.SimpleInjector;
using SimpleInjector;

internal class CompositionRoot
{
    public static IDependencyInjectionContainer Compose()
    {
        // Create the DI container
        var container = new Container();

        // Setup configuration of DI
        MvcSiteMapProviderContainerInitializer.SetUp(container);

#if DEBUG // preserve
        container.Verify();
#endif // preserve

        // Return our DI container wrapper instance
        return new SimpleInjectorDependencyInjectionContainer(container);
    }
}