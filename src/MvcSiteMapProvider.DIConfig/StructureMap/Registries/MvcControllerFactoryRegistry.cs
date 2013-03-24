using System;
using System.Web.Mvc;
using StructureMap.Configuration.DSL;
using MvcSiteMapProvider.Web.Mvc;
using DI.StructureMap.Conventions;

namespace DI.StructureMap.Registries
{
    internal class MvcControllerFactoryRegistry
        : Registry
    {
        public MvcControllerFactoryRegistry()
        {
            // Fix for controllers - need to ensure they are transient or http scoped or
            // there will be problems when using IControllerFactory.
            //http://code-inside.de/blog/2011/01/18/fix-a-single-instance-of-controller-foocontroller-cannot-be-used-to-handle-multiple-requests-mvc3/
            this.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.AssemblyContainingType<XmlSiteMapController>();
                scan.AddAllTypesOf<IController>();
                scan.Include(t => typeof(IController).IsAssignableFrom(t));
                scan.Convention<TransientConvention>();
            });

            this.For<IControllerFactory>()
                .Singleton()
                .Use<InjectableControllerFactory>();
        }
    }
}
