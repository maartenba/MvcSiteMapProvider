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

        // TODO: Add DIConfigBootstrapper.Start() to Global.asax file under Application_Start().
#endif
        var container = DIConfig.Register();

#if DependencyResolver
        // Reconfigure MVC to use Service Location
        var dependencyResolver = new InjectableDependencyResolver(container);
        DependencyResolver.SetResolver(dependencyResolver);
#else
#if !MVC2
        // ************************************************************************************** //
        //  Dependency Resolver
        //
        //  You may use this option if you are using MVC 3 or higher and you have other code
        //  that references DependencyResolver.Current.GetService() or DependencyResolver.Current.GetServices()
        //
        // ************************************************************************************** //

        // TODO: To use Dependency resolver, uncomment the following lines and 
        // comment the controller factory lines below
        //// Reconfigure MVC to use Service Location
        //var dependencyResolver = new InjectableDependencyResolver(container);
        //DependencyResolver.SetResolver(dependencyResolver);

        // ************************************************************************************** //
        //  Controller Factory
        //
        //  It is recommended to use Controller Factory unless you are getting errors due to a conflict.
        //
        // ************************************************************************************** //
#endif
        // Reconfigure MVC to use DI
        var controllerFactory = new InjectableControllerFactory(container);
        ControllerBuilder.Current.SetControllerFactory(controllerFactory);
#endif

        MvcSiteMapProviderConfig.Register(container);
    }
}

