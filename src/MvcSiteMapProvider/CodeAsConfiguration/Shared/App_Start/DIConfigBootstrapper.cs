using System;
using System.Web.Mvc;
using DI;

#if !NET35
[assembly: WebActivatorEx.PostApplicationStartMethod(typeof(DIConfigBootstrapper), "Start")]
#endif

public class DIConfigBootstrapper
{
    public static void Start()
    {
#if NET35
        MvcSiteMapProvider.DI.Composer.Compose();

        // TODO: Add DIBootstrapper.Start() to Global.asax file under Application_Start().
#endif
        var container = DIConfig.Register();


#if DependencyResolver

        // ************************************************************************************** //
        //  Dependency Resolver
        //
        //  You may use this option if you are using MVC 3 or higher and you have other code
        //  that references DependencyResolver.Current.GetService() or DependencyResolver.Current.GetServices()
        //
        // ************************************************************************************** //

        // Reconfigure MVC to use Service Location

        // You will need to use DependencyResolver if there are hard references
        // to IDependencyResolver in your code somewhere (don't do that - it limits your options
        // and tightly couples your code to MVC!).
        var dependencyResolver = new InjectableDependencyResolver(container);
        DependencyResolver.SetResolver(dependencyResolver);

#else
        // ************************************************************************************** //
        //  Controller Factory
        //
        //  You MUST use this option if you are using MVC 2.
        //
        //  It is recommended to use Controller Factory unless you are getting errors due to a conflict.
        //
        // ************************************************************************************** //

        // Reconfigure MVC to use DI
        var controllerFactory = new InjectableControllerFactory(container);
        ControllerBuilder.Current.SetControllerFactory(controllerFactory);
#endif
        MvcSiteMapProviderConfig.Register(container);
    }
}

