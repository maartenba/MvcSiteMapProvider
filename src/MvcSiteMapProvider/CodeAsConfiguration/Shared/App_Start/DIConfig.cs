using System;
using System.Web.Mvc;
using DI;

#if !NET35
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(DIConfig), "Register")]
#else
    // TODO: Add DIConfig.Register(); to Global.asax file under Application_Start().
#endif


public class DIConfig
{
    public static void Register()
    {
#if NET35
        MvcSiteMapProvider.DI.Composer.Compose();
#endif
#if Demo
        if (new MvcSiteMapProvider.DI.ConfigurationSettings().UseExternalDIContainer)
        {
#endif
        var container = CompositionRoot.Compose();
#if !MVC2
#if DependencyResolver //preserve
        // ************************************************************************************** //
        //  Dependency Resolver
        //
        //  You may use this option if you are using MVC 3 or higher and you have other code
        //  that references DependencyResolver.Current.GetService() or DependencyResolver.Current.GetServices()
        //
        // ************************************************************************************** //

        // Reconfigure MVC to use Service Location
        var dependencyResolver = new InjectableDependencyResolver(container, DependencyResolver.Current);
        DependencyResolver.SetResolver(dependencyResolver);
#else //preserve
        // ************************************************************************************** //
        //  Controller Factory
        //
        //  It is recommended to use Controller Factory unless you are getting errors due to a conflict.
        //
        // ************************************************************************************** //

        // Reconfigure MVC to use DI
        var controllerFactory = new InjectableControllerFactory(container);
        ControllerBuilder.Current.SetControllerFactory(controllerFactory);
#endif //preserve
#else
        // Reconfigure MVC to use DI
        var controllerFactory = new InjectableControllerFactory(container);
        ControllerBuilder.Current.SetControllerFactory(controllerFactory);
#endif
        MvcSiteMapProviderConfig.Register(container);
#if Demo
        }
#endif
    }
}

