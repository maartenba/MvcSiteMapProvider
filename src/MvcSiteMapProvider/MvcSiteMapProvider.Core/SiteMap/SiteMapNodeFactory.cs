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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapNodeFactory 
        : ISiteMapNodeFactory
    {
        public SiteMapNodeFactory(
            //ISiteMapNodeFactory siteMapNodeFactory, 
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy,
            ISiteMapNodeUrlResolverStrategy siteMapNodeUrlResolverStrategy
            ) 
        {
            //if (siteMapNodeFactory == null)
            //    throw new ArgumentNullException("siteMapNodeFactory");
            if (dynamicNodeProviderStrategy == null)
                throw new ArgumentNullException("dynamicNodeProviderStrategy");
            if (siteMapNodeUrlResolverStrategy == null)
                throw new ArgumentNullException("siteMapNodeUrlResolverStrategy");

            //this.siteMapNodeFactory = siteMapNodeFactory;
            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
            this.siteMapNodeUrlResolverStrategy = siteMapNodeUrlResolverStrategy;
        }

        // Services
        //protected readonly ISiteMapNodeFactory siteMapNodeFactory;
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
                //siteMapNodeFactory, 
                dynamicNodeProviderStrategy,
                siteMapNodeUrlResolverStrategy);
        }

        public ISiteMapNode CreateDynamic(ISiteMap siteMap, string implicitResourceKey)
        {
            return new SiteMapNode(
                siteMap, 
                string.Empty, 
                implicitResourceKey, 
                //siteMapNodeFactory, 
                dynamicNodeProviderStrategy,
                siteMapNodeUrlResolverStrategy);
        }

        #endregion
    }
}
