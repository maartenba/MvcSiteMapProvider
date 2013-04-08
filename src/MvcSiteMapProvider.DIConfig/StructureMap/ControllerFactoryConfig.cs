using System;
using System.Web.Mvc;
using DI;


internal class ControllerFactoryConfig
{
    public static void Register(IDependencyInjectionContainer container)
    {
        // Reconfigure MVC to use DI
        ControllerBuilder.Current.SetControllerFactory(new InjectableControllerFactory(container));
    }
}
