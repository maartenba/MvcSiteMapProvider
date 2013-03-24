using System;

namespace DI
{
    public interface IDependencyInjectionContainer
    {
        object Resolve(Type type);
        T Resolve<T>();
    }
}