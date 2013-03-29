using System;
using StructureMap;
using StructureMap.Configuration.DSL;

namespace DI.StructureMap.Registries
{
    internal class StructureMapRegistry
        : Registry
    {
        public StructureMapRegistry(IContainer container)
        {
            this.For<IDependencyInjectionContainer>()
                .Singleton()
                .Use<StructureMapContainer>()
                .Ctor<IContainer>().Is(container);
        }
    }
}
