using System;
using System.Web.Mvc;
using System.Web.Routing;
using MvcSiteMapProvider.Web.Mvc;
using DI;


#if !NET35
    [assembly: WebActivatorEx.PostApplicationStartMethod(typeof(DI_ControllerFactory_Bootstrapper), "Start")]
#endif

public class DI_ControllerFactory_Bootstrapper
{
    public static void Start()
    {
        // MvcSiteMapProvider Configuration
#if NET35
        MvcSiteMapProvider.DI.Composer.Compose();

        // TODO: Add DI_ControllerFactory_Bootstrapper.Start() to Global.asax file under Application_Start().
#endif
        var container = DIConfig.Register();

        // Reconfigure MVC to use DI
        var controllerFactory = new InjectableControllerFactory(container);
        ControllerBuilder.Current.SetControllerFactory(controllerFactory);

        MvcSiteMapProviderConfig.Register(container);

        XmlSiteMapController.RegisterRoutes(RouteTable.Routes);
    }
}

