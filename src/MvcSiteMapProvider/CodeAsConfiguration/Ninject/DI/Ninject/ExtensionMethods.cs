using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DI.Ninject
{
    public static class ExtensionMethods
    {
        public static IList<Type> GetImplementationsOfInterface(this Assembly assembly, Type interfaceType)
        {
            var implementations = new List<Type>();

            var concreteTypes = assembly.GetTypes().Where(t =>
                !t.IsInterface &&
                !t.IsAbstract &&
                interfaceType.IsAssignableFrom(t));

            concreteTypes.ToList().ForEach(implementations.Add);

            return implementations;
        }
    }
}
