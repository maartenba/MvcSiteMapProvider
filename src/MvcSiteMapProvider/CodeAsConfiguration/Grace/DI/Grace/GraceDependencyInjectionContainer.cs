using System;
using System.Collections.Generic;
using Grace.DependencyInjection;

namespace DI.Grace
{
    internal class GraceDependencyInjectionContainer
        : IDependencyInjectionContainer
    {
        public GraceDependencyInjectionContainer(IExportLocator container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
        }
		  private readonly IExportLocator container;

        public object GetInstance(Type type)
        {
            return container.Locate(type);
        }

        public object TryGetInstance(Type type)
        {
            try
            {
                return container.Locate(type);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            return container.LocateAll(type);
        }

        public void Release(object instance)
        {
            // Ddo nothing
        }
    }
}
