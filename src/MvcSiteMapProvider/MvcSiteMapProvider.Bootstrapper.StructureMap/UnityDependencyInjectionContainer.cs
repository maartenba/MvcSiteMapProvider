using System;
using System.Collections.Generic;
using Microsoft.Practices.StructureMap;

namespace MvcSiteMapProvider.Bootstrapper.StructureMap
{
    public class StructureMapDependencyInjectionContainer
        : IDependencyInjectionContainer
    {
        public StructureMapDependencyInjectionContainer(
            IStructureMapContainer container
            )
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        private readonly IStructureMapContainer container;

        #region IDependencyInjectionContainer Members

        public object GetInstance(Type type)
        {
            return this.container.Resolve(type);
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            return this.container.ResolveAll(type);
        }

        #endregion
    }
}