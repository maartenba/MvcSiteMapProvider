using System;
using StructureMap;
using StructureMap.Graph;
using StructureMap.Configuration.DSL;

namespace DI.StructureMap.Conventions
{
    internal class TransientConvention
        : IRegistrationConvention
    {
        #region IRegistrationConvention Members

        public void Process(Type type, Registry registry)
        {
            registry.For(type).LifecycleIs(InstanceScope.Transient);
        }

        #endregion
    }
}
