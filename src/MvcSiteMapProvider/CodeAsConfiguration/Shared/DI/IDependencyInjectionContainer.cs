using System;
using System.Collections.Generic;

namespace DI
{
    public interface IDependencyInjectionContainer
    {
        object GetInstance(Type type);
        object TryGetInstance(Type type);
        IEnumerable<object> GetAllInstances(Type type);
        void Release(object instance);
    }

    public static class DependencyInjectionContainerExtensions
    {
        public static T GetInstance<T>(this IDependencyInjectionContainer container)
        {
            return (T)container.GetInstance(typeof(T));
        }
    }
}
