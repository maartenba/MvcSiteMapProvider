using System;
using CommonServiceLocator.StructureMapAdapter.Unofficial;
using Microsoft.Practices.ServiceLocation;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace DI.StructureMap.Registries
{
    internal class StructureMapRegistry
        : Registry
    {
        public StructureMapRegistry(IContainer container)
        {
            this.For<IServiceLocator>()
                .Singleton()
                .Use<StructureMapServiceLocator>()
                .Ctor<IContainer>().Is(container);
        }
    }
}
