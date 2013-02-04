using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Core.Mvc.UrlResolver;
using MvcSiteMapProvider.Core.Globalization;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapNodeFactory 
        : ISiteMapNodeFactory
    {
        public SiteMapNodeFactory(
            IExplicitResourceKeyParser explicitResourceKeyParser,
            IStringLocalizer stringLocalizer,
            IAttributeCollectionFactory attributeCollectionFactory,
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy
            ) 
        {
            if (explicitResourceKeyParser == null)
                throw new ArgumentNullException("explicitResourceKeyParser");
            if (stringLocalizer == null)
                throw new ArgumentNullException("stringLocalizer");
            if (attributeCollectionFactory == null)
                throw new ArgumentNullException("attributeCollectionFactory");
            if (dynamicNodeProviderStrategy == null)
                throw new ArgumentNullException("dynamicNodeProviderStrategy");
            if (siteMapNodeUrlResolverStrategy == null)
                throw new ArgumentNullException("siteMapNodeUrlResolverStrategy");

            this.explicitResourceKeyParser = explicitResourceKeyParser;
            this.stringLocalizer = stringLocalizer;
            this.attributeCollectionFactory = attributeCollectionFactory;
            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
            this.siteMapNodeUrlResolverStrategy = siteMapNodeUrlResolverStrategy;
        }

        // Services
        protected readonly IExplicitResourceKeyParser explicitResourceKeyParser;
        protected readonly IStringLocalizer stringLocalizer;
        protected readonly IAttributeCollectionFactory attributeCollectionFactory;
        protected readonly IDynamicNodeProviderStrategy dynamicNodeProviderStrategy;
        protected readonly ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy;


        #region ISiteMapNodeFactory Members

        public ISiteMapNode Create(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            return CreateInternal(siteMap, key, implicitResourceKey, false);
        }

        public ISiteMapNode CreateDynamic(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            return CreateInternal(siteMap, key, implicitResourceKey, true);
        }

        protected ISiteMapNode CreateInternal(ISiteMap siteMap, string key, string implicitResourceKey, bool isDynamic)
        {
            // IMPORTANT: we must create one localization service per node because the service contains its own state that applies to the node
            var localizationService = CreateLocalizationService(implicitResourceKey);
            var attributes = CreateAttributeCollection(siteMap, localizationService);

            return new SiteMapNode(
                siteMap,
                key,
                isDynamic,
                attributes,
                localizationService,
                dynamicNodeProviderStrategy,
                siteMapNodeUrlResolverStrategy);
        }


        protected virtual ILocalizationService CreateLocalizationService(string implicitResourceKey)
        {
            return new LocalizationService(implicitResourceKey, explicitResourceKeyParser, stringLocalizer);
        }

        protected virtual IDictionary<string, string> CreateAttributeCollection(ISiteMap siteMap, ILocalizationService localizationService)
        {
            return attributeCollectionFactory.Create(siteMap, localizationService);
        }

        #endregion
    }
}
