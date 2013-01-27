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

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapNodeFactory : ISiteMapNodeFactory
    {
        // TODO: Add constructor to inject services into SiteMapNode
        public SiteMapNodeFactory(
            ISiteMapNodeFactory siteMapNodeFactory, 
            IDynamicNodeProviderStrategy dynamicNodeProviderStrategy
            ) 
        {
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");
            if (dynamicNodeProviderStrategy == null)
                throw new ArgumentNullException("dynamicNodeProviderStrategy");

            this.siteMapNodeFactory = siteMapNodeFactory;
            this.dynamicNodeProviderStrategy = dynamicNodeProviderStrategy;
        }

        // Services
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;
        protected readonly IDynamicNodeProviderStrategy dynamicNodeProviderStrategy;


        #region ISiteMapNodeFactory Members

        public ISiteMapNode Create(ISiteMap siteMap, string key, string implicitResourceKey)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException("key");

            return new SiteMapNode2(
                siteMap, 
                key, 
                implicitResourceKey, 
                siteMapNodeFactory, 
                dynamicNodeProviderStrategy);
        }

        public ISiteMapNode CreateDynamic(ISiteMap siteMap, string implicitResourceKey)
        {
            return new SiteMapNode2(
                siteMap, 
                string.Empty, 
                implicitResourceKey, 
                siteMapNodeFactory, 
                dynamicNodeProviderStrategy);
        }

        #endregion
    }
}
