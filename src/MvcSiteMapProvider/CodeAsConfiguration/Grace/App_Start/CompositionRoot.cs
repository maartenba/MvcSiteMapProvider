using System;
using DI;
using DI.Grace;
using DI.Grace.Modules;

internal class CompositionRoot
{
    public static IDependencyInjectionContainer Compose()
    {
        // Create new container
        var container = new Grace.DependencyInjection.DependencyInjectionContainer();

        // Install MVC sitemap provider
        container.Configure(new MvcSiteMapProviderModule());

        // Install Controllers
        container.Configure(new MvcModule());

        // Add your DI configuration here

        return new GraceDependencyInjectionContainer(container.RootScope);
    }
}
