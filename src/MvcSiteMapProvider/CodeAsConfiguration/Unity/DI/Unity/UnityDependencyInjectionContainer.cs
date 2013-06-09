using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.Practices.Unity;
using DI;

namespace DI.Unity
{
    public class UnityDependencyInjectionContainer
        : IDependencyInjectionContainer
    {
        public UnityDependencyInjectionContainer(
            IUnityContainer container
            )
        {
            if (container == null)
                throw new ArgumentNullException("container");
            this.container = container;
        }

        private readonly IUnityContainer container;

        public object GetInstance(Type type)
        {
            return this.container.Resolve(type);
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            return this.container.ResolveAll(type);
        }

        public void Release(object instance)
        {
            // Do nothing
        }
    }
}
