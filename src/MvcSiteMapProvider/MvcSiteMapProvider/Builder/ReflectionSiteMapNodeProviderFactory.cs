using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Abstract factory to assist with the creation of ReflectionSiteMapNodeProvider for DI containers 
    /// that don't support injection of a partial list of constructor parameters. Without using this 
    /// class, DI configuration code for those containers is very brittle.
    /// </summary>
    public class ReflectionSiteMapNodeProviderFactory
    {
        public ReflectionSiteMapNodeProviderFactory(
            IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory,
            IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider
            )
        {
            if (attributeAssemblyProviderFactory == null)
                throw new ArgumentNullException("attributeAssemblyProviderFactory");
            if (attributeNodeDefinitionProvider == null)
                throw new ArgumentNullException("attributeNodeDefinitionProvider");

            this.attributeAssemblyProviderFactory = attributeAssemblyProviderFactory;
            this.attributeNodeDefinitionProvider = attributeNodeDefinitionProvider;
        }
        protected readonly IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider;
        protected readonly IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory;

        public ReflectionSiteMapNodeProvider Create(IEnumerable<String> includeAssemblies, IEnumerable<String> excludeAssemblies)
        {
            return new ReflectionSiteMapNodeProvider(
                includeAssemblies, 
                excludeAssemblies, 
                this.attributeAssemblyProviderFactory, 
                this.attributeNodeDefinitionProvider);
        }

        public ReflectionSiteMapNodeProvider Create(IEnumerable<String> includeAssemblies)
        {
            return new ReflectionSiteMapNodeProvider(
                includeAssemblies, 
                new string[0], 
                this.attributeAssemblyProviderFactory, 
                this.attributeNodeDefinitionProvider);
        }
    }
}
