using Microsoft.Practices.Unity;
using $rootnamespace$.DI.MvcSiteMapProvider.ContainerExtensions;
using MvcSiteMapProvider.DI.Bootstrap;
using $rootnamespace$.DI.MvcSiteMapProvider.ContainerExtensions;

namespace $rootnamespace$.DI.MvcSiteMapProvider
{
    internal class DIConfig
    {
        public static IDependencyInjectionContainer Register()
        {
            var container = new UnityContainer();
            container.AddNewExtension<MvcSiteMapProviderContainerExtension>();

            return new UnityDependencyInjectionContainer(container);
        }
    }
}
