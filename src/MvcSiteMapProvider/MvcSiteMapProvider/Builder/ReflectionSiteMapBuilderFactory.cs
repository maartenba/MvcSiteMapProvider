using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Reflection;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Abstract factory to assist with the creation of ReflectionSiteMapBuilder for DI containers 
    /// that don't support injection of a partial list of constructor parameters.
    /// </summary>
    public class ReflectionSiteMapBuilderFactory
        : IReflectionSiteMapBuilderFactory
    {
        public ReflectionSiteMapBuilderFactory(
            ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider,
            ISiteMapAssemblyService siteMapAssemblyService,
            IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory,
            IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider
            )
        {
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (siteMapAssemblyService == null)
                throw new ArgumentNullException("siteMapAssemblyService");
            if (attributeAssemblyProviderFactory == null)
                throw new ArgumentNullException("attributeAssemblyProviderFactory");
            if (attributeNodeDefinitionProvider == null)
                throw new ArgumentNullException("attributeNodeDefinitionProvider");

            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.siteMapAssemblyService = siteMapAssemblyService;
            this.attributeAssemblyProviderFactory = attributeAssemblyProviderFactory;
            this.attributeNodeDefinitionProvider = attributeNodeDefinitionProvider;
        }
        protected readonly ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly ISiteMapAssemblyService siteMapAssemblyService;
        protected readonly IMvcSiteMapNodeAttributeDefinitionProvider attributeNodeDefinitionProvider;
        protected readonly IAttributeAssemblyProviderFactory attributeAssemblyProviderFactory;

        #region IReflectionSiteMapBuilderFactory Members

        public ISiteMapBuilder Create(IEnumerable<string> includeAssemblies)
        {
            return new ReflectionSiteMapBuilder(
                includeAssemblies,
                new string[0],
                this.reservedAttributeNameProvider,
                this.siteMapAssemblyService,
                this.attributeAssemblyProviderFactory,
                this.attributeNodeDefinitionProvider);
        }

        public ISiteMapBuilder Create(IEnumerable<string> includeAssemblies, IEnumerable<string> excludeAssemblies)
        {
            return new ReflectionSiteMapBuilder(
                includeAssemblies,
                excludeAssemblies,
                this.reservedAttributeNameProvider,
                this.siteMapAssemblyService,
                this.attributeAssemblyProviderFactory,
                this.attributeNodeDefinitionProvider);
        }

        #endregion
    }
}
