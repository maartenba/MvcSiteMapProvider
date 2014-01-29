using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Web.Script.Serialization;
using MvcSiteMapProvider.Xml;

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
            IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider,
            IJavaScriptSerializer javaScriptSerializer
            )
        {
            if (attributeAssemblyProviderFactory == null)
                throw new ArgumentNullException("attributeAssemblyProviderFactory");
            if (attributeNodeDefinitionProvider == null)
                throw new ArgumentNullException("attributeNodeDefinitionProvider");
            if (javaScriptSerializer == null)
                throw new ArgumentNullException("javaScriptSerializer");

            this.attributeAssemblyProviderFactory = attributeAssemblyProviderFactory;
            this.attributeNodeDefinitionProvider = attributeNodeDefinitionProvider;
            this.javaScriptSerializer = javaScriptSerializer;
        }
        protected readonly IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider;
        protected readonly IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory;
        protected readonly IJavaScriptSerializer javaScriptSerializer;

        public ReflectionSiteMapNodeProvider Create(IEnumerable<String> includeAssemblies, IEnumerable<String> excludeAssemblies)
        {
            return new ReflectionSiteMapNodeProvider(
                includeAssemblies, 
                excludeAssemblies, 
                this.attributeAssemblyProviderFactory, 
                this.attributeNodeDefinitionProvider,
                this.javaScriptSerializer);
        }

        public ReflectionSiteMapNodeProvider Create(IEnumerable<String> includeAssemblies)
        {
            return new ReflectionSiteMapNodeProvider(
                includeAssemblies, 
                new string[0], 
                this.attributeAssemblyProviderFactory, 
                this.attributeNodeDefinitionProvider,
                this.javaScriptSerializer);
        }
    }
}
