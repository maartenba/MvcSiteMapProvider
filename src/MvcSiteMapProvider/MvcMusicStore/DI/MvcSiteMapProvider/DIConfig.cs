using MvcMusicStore.DI.MvcSiteMapProvider.Registries;
using MvcSiteMapProvider.DI.Bootstrap;
using StructureMap;

namespace MvcMusicStore.DI.MvcSiteMapProvider
{
    public class DIConfig
    {
        public static IDependencyInjectionContainer Register()
        {
            // Create the DI container
            var container = new Container();

            // Setup configuration of DI
            container.Configure(r => r.AddRegistry<MvcSiteMapProviderRegistry>());
            container.Configure(r => r.AddRegistry<MvcRegistry>());

            // Verify the configuration
            // TODO: Move this into a test
            //container.AssertConfigurationIsValid();

            // Return our DI container wrapper instance
            return new StructureMapContainer(container);
        }
    }
}
