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
        private readonly IDependencyResolver dependencyResolver;

        public InjectableDependencyResolver(IDependencyInjectionContainer container, IDependencyResolver currentDependencyResolver)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            if (currentDependencyResolver == null)
                throw new ArgumentNullException("currentDependencyResolver");
            this.container = container;
            this.dependencyResolver = currentDependencyResolver;
        }

        public object GetService(Type serviceType)
        {
            object result = container.TryGetInstance(serviceType);
            if (result == null)
            {
                result = this.dependencyResolver.GetService(serviceType);
            }
            return result;
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return container.GetAllInstances(serviceType);
        }
    }
}
#endif