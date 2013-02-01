// -----------------------------------------------------------------------
// <copyright file="TypeExtensions.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.Reflection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Reflection;

    /// <summary>
    /// TODO: Update summary.
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
