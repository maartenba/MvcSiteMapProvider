using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using MvcSiteMapProvider.DI.Bootstrap;
using $rootnamespace$.DI.MvcSiteMapProvider.ContainerExtensions;

namespace $rootnamespace$.DI.MvcSiteMapProvider
{
    public class UnityDependencyInjectionContainer
        : IDependencyInjectionContainer
    {
        public UnityDependencyInjectionContainer(
            IUnityContainer container
            )
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }
            this.container = container;
        }

        private readonly IUnityContainer container;

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