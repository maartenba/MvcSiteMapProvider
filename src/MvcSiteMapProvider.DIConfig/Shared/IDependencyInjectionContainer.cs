using System;

namespace DI
{
    public interface IDependencyInjectionContainer
    {
        object Resolve(Type type);
    }

    public static class DependencyInjectionContainerExtensions
    {
        public static T Resolve<T>(this IDependencyInjectionContainer container)
        {
            return (T)container.Resolve(typeof(T));
        }
    }
}