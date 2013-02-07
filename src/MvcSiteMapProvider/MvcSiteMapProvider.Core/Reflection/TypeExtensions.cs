using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace MvcSiteMapProvider.Core.Reflection
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
