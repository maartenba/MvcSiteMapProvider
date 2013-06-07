using Autofac.Core;
using $rootnamespace$.DI.MvcSiteMapProvider.Modules;
using MvcSiteMapProvider.DI.Bootstrap;
using Autofac;

namespace $rootnamespace$.DI.MvcSiteMapProvider
{
    public class DIConfig
    {
        public static IDependencyInjectionContainer Register()
        {
            // Create a container builder
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MvcSiteMapProviderModule());
            builder.RegisterModule(new MvcModule());

            // Create the DI container
            var container = builder.Build();

            // Return our DI container wrapper instance
            return new AutofacContainer(container);
        }
    }
}
