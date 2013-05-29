using System.Web.Mvc;

namespace MvcSiteMapProvider.DI.Bootstrap
{
    public class ControllerFactoryConfig
    {
        public static void Register(IDependencyInjectionContainer container)
        {
            // Reconfigure MVC to use DI
            var controllerFactory = new InjectableControllerFactory(container);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}