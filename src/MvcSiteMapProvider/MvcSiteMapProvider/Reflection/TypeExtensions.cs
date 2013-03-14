using System;
using System.Reflection;

namespace MvcSiteMapProvider.Reflection
{
    
    /// <summary>
    /// Extensions to the Type type.
    /// </summary>
    public static class TypeExtensions
    {
        public static string ShortAssemblyQualifiedName(this Type type)
        {
            var assemblyName = new AssemblyName(type.Assembly.FullName);
            var shortName = type.FullName + ", " + assemblyName.Name;
            return shortName;
        }
    }
}
