using Microsoft.Practices.Unity;
using MvcSiteMapProvider.Bootstrapper.Unity.ContainerExtensions;

namespace MvcSiteMapProvider.Bootstrapper.Unity
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
