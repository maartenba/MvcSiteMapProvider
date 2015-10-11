using MvcSiteMapProvider.DI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MvcSiteMapProvider.Reflection
{
    [ExcludeFromAutoRegistration]
    public class AttributeAssemblyProvider
        : IAttributeAssemblyProvider
    {
        public AttributeAssemblyProvider(
            IEnumerable<string> includeAssemblies,
            IEnumerable<string> excludeAssemblies)
        {
            if (includeAssemblies == null)
                throw new ArgumentNullException("includeAssemblies");
            if (excludeAssemblies == null)
                throw new ArgumentNullException("excludeAssemblies");

            this.includeAssemblies = includeAssemblies;
            this.excludeAssemblies = excludeAssemblies;
        }
        protected readonly IEnumerable<string> includeAssemblies;
        protected readonly IEnumerable<string> excludeAssemblies;

        #region IAttributeAssemblyProvider Members

        public IEnumerable<System.Reflection.Assembly> GetAssemblies()
        {
            // List of assemblies
            IEnumerable<Assembly> assemblies;
            if (includeAssemblies.Any())
            {
                // An include list is given
                assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => includeAssemblies.Contains(new AssemblyName(a.FullName).Name));
            }
            else
            {
                // An exclude list is given
                assemblies = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(a => !a.FullName.StartsWith("mscorlib")
                                && !a.FullName.StartsWith("System")
                                && !a.FullName.StartsWith("Microsoft")
                                && !a.FullName.StartsWith("WebDev")
                                && !a.FullName.StartsWith("SMDiagnostics")
                                && !a.FullName.StartsWith("Anonymously")
                                && !a.FullName.StartsWith("App_")
                                && !excludeAssemblies.Contains(new AssemblyName(a.FullName).Name));
            }

            // http://stackoverflow.com/questions/1423733/how-to-tell-if-a-net-assembly-is-dynamic
            return assemblies
                .Where(a =>
                    !(a.ManifestModule is System.Reflection.Emit.ModuleBuilder)
                    && a.ManifestModule.GetType().Namespace != "System.Reflection.Emit"
                );
        }

        #endregion
    }
}
