using System;
using System.Web.Mvc;
using StructureMap.Configuration.DSL;
using MvcSiteMapProvider.Web.Mvc;
using DI.StructureMap.Conventions;

namespace DI.StructureMap.Registries
{
    internal class MvcDependencyResolverRegistry
        : Registry
    {
        public MvcDependencyResolverRegistry()
        {
            this.For<IDependencyResolver>()
                .Singleton()
                .Use<InjectableDependencyResolver>();
        }
    }
}
