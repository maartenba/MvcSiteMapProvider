using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DI
{
    internal class CommonConventions
    {
        // Single implementations of interface with matching name (minus the "I").
        public static void RegisterDefaultConventions(
            Action<Type, Type> registerMethod, 
            Assembly[] interfaceAssemblies, 
            Assembly[] implementationAssemblies, 
            Type[] excludeTypes,
            string excludeRegEx)
        {
            List<Type> interfaces = new List<Type>();

            foreach (var assembly in interfaceAssemblies)
                interfaces.AddRange(GetInterfaces(assembly));

            foreach (var interfaceType in interfaces)
            {
                if (!IsMatch(interfaceType, excludeTypes, excludeRegEx))
                {
                    List<Type> implementations = new List<Type>();

                    foreach (var assembly in implementationAssemblies)
                        implementations.AddRange(GetImplementationsOfInterface(assembly, interfaceType));

                    if (implementations.Count == 1)
                    {
                        var implementationType = implementations[0];
                        if (!IsMatch(implementationType, excludeTypes, excludeRegEx) && interfaceType.Name.Equals("I" + implementationType.Name))
                        {
                            System.Diagnostics.Debug.WriteLine("Auto registration of {1} : {0}", interfaceType.Name, implementationType.Name);
                            registerMethod(interfaceType, implementationType);
                        }
                    }
                }
            }
        }

        // For DI containers that allow the use of a multiple registration method calls for individual implementations of a given interface
        public static void RegisterAllImplementationsOfInterface(
            Action<Type, Type> registerMethod,
            Type[] interfaceTypes,
            Assembly[] implementationAssemblies,
            Type[] excludeTypes,
            string excludeRegEx)
        {
            foreach (var interfaceType in interfaceTypes)
            {
                List<Type> implementations = new List<Type>();

                foreach (var assembly in implementationAssemblies)
                    implementations.AddRange(GetImplementationsOfInterface(assembly, interfaceType));

                foreach (var implementationType in implementations)
                {
                    if (!IsMatch(implementationType, excludeTypes, excludeRegEx))
                    {
                        System.Diagnostics.Debug.WriteLine("Auto registration of {1} : {0}", interfaceType.Name, implementationType.Name);
                        registerMethod(interfaceType, implementationType);
                    }
                }
            }
        }

        // For DI containers that require the use of a single registration method call for all implementations of a given interface
        public static void RegisterAllImplementationsOfInterfaceSingle(
            Action<Type, IEnumerable<Type>> registerMethod,
            Type[] interfaceTypes,
            Assembly[] implementationAssemblies,
            Type[] excludeTypes,
            string excludeRegEx)
        {
            foreach (var interfaceType in interfaceTypes)
            {
                List<Type> implementations = new List<Type>();
                List<Type> matchingImplementations = new List<Type>();

                foreach (var assembly in implementationAssemblies)
                    implementations.AddRange(GetImplementationsOfInterface(assembly, interfaceType));

                foreach (var implementationType in implementations)
                {
                    if (!IsMatch(implementationType, excludeTypes, excludeRegEx))
                    {
                        matchingImplementations.Add(implementationType);
                    }
                }

                System.Diagnostics.Debug.WriteLine("Auto multiple registration of {1} : {0}", interfaceType.Name, string.Join(", ", matchingImplementations.Select(t => t.Name)));
                registerMethod(interfaceType, matchingImplementations);
            }
        }


        private static bool IsMatch(Type type, Type[] excludeTypes, string excludeRegEx)
        {
            return IsMatch(type, excludeTypes) || IsMatch(type, excludeRegEx);
        }

        private static bool IsMatch(Type type, Type[] excludeTypes)
        {
            return excludeTypes.Contains(type);
        }

        private static bool IsMatch(Type type, string excludeRegEx)
        {
            if (string.IsNullOrEmpty(excludeRegEx)) return false;
            return Regex.Match(type.Name, excludeRegEx, RegexOptions.Compiled).Success;
        }

        private static IEnumerable<Type> GetInterfaces(Assembly assembly)
        {
            return assembly.GetTypes().Where(t => t.IsInterface);
        }

        private static IEnumerable<Type> GetImplementationsOfInterface(Assembly assembly, Type interfaceType)
        {
            return assembly.GetTypes().Where(t =>
                !t.IsInterface &&
                !t.IsAbstract &&
                interfaceType.IsAssignableFrom(t) &&
                t.GetConstructors(BindingFlags.Public | BindingFlags.Instance)
                    .Any(type => type.GetParameters().Select(p => p.ParameterType).All(p => (p.IsInterface || p.IsClass) && p != typeof(string))));
        }
    }
}
