using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Mvc;
using DI;

#if !NET35
    [assembly: WebActivatorEx.PostApplicationStartMethod(typeof(DI_DependencyResolver_Bootstrapper), "Start")]
#endif

public class DI_DependencyResolver_Bootstrapper
{
    public static void Start()
    {
        // MvcSiteMapProvider Configuration
#if NET35
        MvcSiteMapProvider.DI.Composer.Compose();

        // TODO: Add DI_DependencyResolver_Bootstrapper.Start() to Global.asax file under Application_Start().
#endif
        var container = DIConfig.Register();

        // Reconfigure MVC to use Service Location

        // You will need to use DependencyResolver if there are hard references
        // to IDependencyResolver in your code somewhere (don't do that - it limits your options
        // and tightly couples your code to MVC!).
        var dependencyResolver = new InjectableDependencyResolver(container);
        DependencyResolver.SetResolver(dependencyResolver);

        MvcSiteMapProviderConfig.Register(container);

        XmlSiteMapController.RegisterRoutes(RouteTable.Routes);
    }
}

