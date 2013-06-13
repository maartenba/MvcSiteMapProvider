#if !MVC2
using System;
using System.Collections.Generic;
using System.Web.Routing;
using System.Web.Mvc;

namespace DI
{
    internal class InjectableDependencyResolver
        : IDependencyResolver
    {
        private readonly IDependencyInjectionContainer container;

        public InjectableDependencyResolver(IDependencyInjectionContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return container.GetInstance(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances(serviceType);
        }
    }
}
#endif