using System;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace $rootnamespace$.DI.MvcSiteMapProvider.Conventions
{
    public class TransientConvention
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
