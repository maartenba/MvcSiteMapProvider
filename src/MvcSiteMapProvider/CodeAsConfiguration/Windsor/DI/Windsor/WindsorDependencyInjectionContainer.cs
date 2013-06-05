using System;
using System.Collections.Generic;
using Castle.Windsor;

namespace DI.Windsor
{
    internal class WindsorDependencyInjectionContainer
        : IDependencyInjectionContainer
    {
        public WindsorDependencyInjectionContainer(IWindsorContainer container)
        {
            if (container == null)
                throw new ArgumentNullException("container");

            this.container = container;
        }
        private readonly IWindsorContainer container;

        #region IDependencyInjectionContainer Members

        public object GetInstance(Type type)
        {
            return container.Resolve(type);
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            return (IEnumerable<object>)container.ResolveAll(type);
        }

        public void Release(object instance)
        {
            container.Release(instance);
        }

        #endregion
    }
}
