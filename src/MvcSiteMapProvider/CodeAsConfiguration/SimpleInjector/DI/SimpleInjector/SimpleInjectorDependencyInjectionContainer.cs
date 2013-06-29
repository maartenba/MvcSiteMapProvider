using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleInjector;

namespace DI.SimpleInjector
{
    public class SimpleInjectorDependencyInjectionContainer : IDependencyInjectionContainer
    {
        private readonly Container _container;

        public SimpleInjectorDependencyInjectionContainer(Container container)
        {
            if (container == null)
                throw new ArgumentNullException("container");
            _container = container;
        }

        public object GetInstance(Type type)
        {
            return _container.GetInstance(type);
        }

        public object TryGetInstance(Type type)
        {
            return ((IServiceProvider) _container).GetService(type);
        }

        public IEnumerable<object> GetAllInstances(Type type)
        {
            return _container.GetAllInstances(type);
        }

        public void Release(object instance)
        {
            // Do nothing
        }
    }
}
