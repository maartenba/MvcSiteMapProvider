using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MvcSiteMapProvider.Builder;

namespace MvcSiteMapProvider.Reflection
{
    public class MvcSiteMapNodeAttributeDefinitionProvider
        : IMvcSiteMapNodeAttributeDefinitionProvider
    {
        #region IMvcSiteMapNodeAttributeDefinitionProvider Members

        public IEnumerable<IMvcSiteMapNodeAttributeDefinition> GetMvcSiteMapNodeAttributeDefinitions(IEnumerable<Assembly> assemblies)
        {
            var result = new List<IMvcSiteMapNodeAttributeDefinition>();
            var types = this.GetTypesFromAssemblies(assemblies);

            foreach (Type type in types)
            {
                result.AddRange(this.GetAttributeDefinitionsForControllers(type));
                result.AddRange(this.GetAttributeDefinitionsForActions(type));
            }
            return result;
        }

        #endregion

        protected virtual IEnumerable<Type> GetTypesFromAssembly(Assembly assembly)
        {
            try
            {
                return assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException ex)
            {
                return ex.Types.Where(t => t != null);
            }
        }

        protected virtual IEnumerable<Type> GetTypesFromAssemblies(IEnumerable<Assembly> assemblies)
        {
            var result = new List<Type>();
            foreach (var assembly in assemblies)
            {
                result.AddRange(this.GetTypesFromAssembly(assembly));
            }
            return result;
        }

        protected virtual IEnumerable<IMvcSiteMapNodeAttributeDefinition> GetAttributeDefinitionsForControllers(Type type)
        {
            var result = new List<IMvcSiteMapNodeAttributeDefinition>();
            var attributes = type.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), true) as IMvcSiteMapNodeAttribute[];
            foreach (var attribute in attributes)
            {
                result.Add(new MvcSiteMapNodeAttributeDefinitionForController
                {
                    SiteMapNodeAttribute = attribute,
                    ControllerType = type
                });
            }
            return result;
        }

        protected virtual IEnumerable<IMvcSiteMapNodeAttributeDefinition> GetAttributeDefinitionsForActions(Type type)
        {
            var result = new List<IMvcSiteMapNodeAttributeDefinition>();
            // Add their methods
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), true).Any());

            foreach (var method in methods)
            {
                var attributes = method.GetCustomAttributes(typeof(IMvcSiteMapNodeAttribute), false) as IMvcSiteMapNodeAttribute[];
                foreach (var attribute in attributes)
                {
                    result.Add(new MvcSiteMapNodeAttributeDefinitionForAction
                    {
                        SiteMapNodeAttribute = attribute,
                        ControllerType = type,
                        ActionMethodInfo = method
                    });
                }
            }
            return result;
        }
    }
}
