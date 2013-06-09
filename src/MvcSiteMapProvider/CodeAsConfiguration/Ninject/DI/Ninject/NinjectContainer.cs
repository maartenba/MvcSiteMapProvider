using System;
using System.Collections.Generic;
using Ninject;

namespace DI.Ninject
{
    public class NinjectContainer
        : IDependencyInjectionContainer
    {
        public NinjectContainer(IKernel container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            this.container = container;
        }
        private readonly IKernel container;

        public object GetInstance(Type type)
        {
            return container.Get(type);
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            return container.GetAll(type);
        }

        public void Release(object instance)
        {
            container.Release(instance);
        }
    }
}
