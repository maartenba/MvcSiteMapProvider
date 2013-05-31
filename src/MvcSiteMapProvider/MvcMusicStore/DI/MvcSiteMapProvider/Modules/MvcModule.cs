using System.Web.Mvc;
using Autofac;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcMusicStore.DI.MvcSiteMapProvider.Registries
{
    public class MvcModule
        : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var currentAssembly = typeof(MvcModule).Assembly;

            builder.RegisterAssemblyTypes(currentAssembly)
                .Where(t => typeof(IController).IsAssignableFrom(t))
                .AsImplementedInterfaces()
                .AsSelf()
                .InstancePerLifetimeScope();
        }

        public MvcModule()
        {
            // Fix for controllers - need to ensure they are transient or http scoped or
            // there will be problems when using dependency injection.
            //http://code-inside.de/blog/2011/01/18/fix-a-single-instance-of-controller-foocontroller-cannot-be-used-to-handle-multiple-requests-mvc3/
            this.

            this.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<XmlSiteMapController>();
                scan.AddAllTypesOf<IController>();
                scan.Include(t => typeof(IController).IsAssignableFrom(t));
                scan.Convention<TransientConvention>();
            });
        }
    }
}
