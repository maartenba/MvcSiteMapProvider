// -----------------------------------------------------------------------
// <copyright file="SiteMapNodeFactory.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MvcSiteMapProvider.Core.Mvc.UrlResolver;
    using MvcSiteMapProvider.Core.Globalization;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapNodeFactory 
        : ISiteMapNodeFactory
    {
        public SiteMapNodeFactory(
            //ISiteMapNodeFactory siteMapNodeFactory, 
            IExplicitResourceKeyParser explicitResourceKeyParser,
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy
            ) 
        {
            //if (siteMapNodeFactory == null)
            //    throw new ArgumentNullException("siteMapNodeFactory");
            if (explicitResourceKeyParser == null)
                throw new ArgumentNullException("explicitResourceKeyParser");
            if (dynamicNodeProviderStrategy == null)
                throw new ArgumentNullException("dynamicNodeProviderStrategy");
            if (siteMapNodeUrlResolverStrategy == null)
                throw new ArgumentNullException("siteMapNodeUrlResolverStrategy");

            //this.siteMapNodeFactory = siteMapNodeFactory;
            this.explicitResourceKeyParser = explicitResourceKeyParser;
            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
            this.siteMapNodeUrlResolverStrategy = siteMapNodeUrlResolverStrategy;
        }

        // Services
        //protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly IExplicitResourceKeyParser explicitResourceKeyParser;
        protected readonly IDynamicNodeProviderStrategy dynamicNodeProviderStrategy;
        protected readonly ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy;


        #region ISiteMapNodeFactory Members

        public ISiteMapNode Create(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            return new SiteMapNode(
                siteMap, 
                key, 
                implicitResourceKey,
                false,
                //siteMapNodeFactory,
                explicitResourceKeyParser,
                dynamicNodeProviderStrategy,
                siteMapNodeUrlResolverStrategy);
        }

        public ISiteMapNode CreateDynamic(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            return new SiteMapNode(
                siteMap,
                key, 
                implicitResourceKey,
                true,
                //siteMapNodeFactory, 
                explicitResourceKeyParser,
                dynamicNodeProviderStrategy,
                siteMapNodeUrlResolverStrategy);
        }

        #endregion
    }
}
