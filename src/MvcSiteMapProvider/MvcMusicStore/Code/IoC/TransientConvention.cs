using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StructureMap.Graph;
using StructureMap.Configuration.DSL;

namespace MvcMusicStore.Code.IoC
{
    internal class TransientConvention
        : IRegistrationConvention
    {
        #region IRegistrationConvention Members

        public void Process(Type type, Registry registry)
        {
            registry.For(type).LifecycleIs(StructureMap.InstanceScope.Transient);
        }

        #endregion
    }
}