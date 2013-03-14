using System;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Web;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.RequestCacheableSiteMapNode"/>
    /// at runtime.
    /// </summary>
    public class SiteMapNodeFactory 
        : ISiteMapNodeFactory
    {
        public SiteMapNodeFactory(
            ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory,
            ILocalizationServiceFactory localizationServiceFactory,
            ISiteMapNodePluginProvider pluginProvider,
            IUrlPath urlPath,
            IMvcContextFactory mvcContextFactory
            ) 
        {
            if (siteMapNodeChildStateFactory == null)
                throw new ArgumentNullException("siteMapNodeChildStateFactory");
            if (localizationServiceFactory == null)
                throw new ArgumentNullException("localizationServiceFactory");
            if (pluginProvider == null)
                throw new ArgumentNullException("pluginProvider");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            if (mvcContextFactory == null)
                throw new ArgumentNullException("mvcContextFactory");

            this.siteMapNodeChildStateFactory = siteMapNodeChildStateFactory;
            this.localizationServiceFactory = localizationServiceFactory;
            this.pluginProvider = pluginProvider;
            this.urlPath = urlPath;
            this.mvcContextFactory = mvcContextFactory;
        }

        // Services
        protected readonly ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory;
        protected readonly ILocalizationServiceFactory localizationServiceFactory;
        protected readonly ISiteMapNodePluginProvider pluginProvider;
        protected readonly IUrlPath urlPath;
        protected readonly IMvcContextFactory mvcContextFactory;


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
            var localizationService = localizationServiceFactory.Create(implicitResourceKey);

            return new RequestCacheableSiteMapNode(
                siteMap,
                key,
                isDynamic,
                pluginProvider,
                mvcContextFactory,
                siteMapNodeChildStateFactory,
                localizationService,
                urlPath);
        }

        #endregion
    }
}
