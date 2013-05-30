namespace ExtensibleSiteMap
{
    using System.Collections.Generic;
    using MvcSiteMapProvider;
    using MvcSiteMapProvider.Globalization;
    using MvcSiteMapProvider.Web;
    using MvcSiteMapProvider.Web.Mvc;

    public class ExtensibleSiteMapNode : MvcSiteMapProvider.SiteMapNode, IExtensibleSiteMapNode
    {
        public ExtensibleSiteMapNode(ISiteMap siteMap, string key, bool isDynamic, ISiteMapNodePluginProvider pluginProvider, IMvcContextFactory mvcContextFactory, ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory, ILocalizationService localizationService, IUrlPath urlPath) 
            : base(siteMap, key, isDynamic, pluginProvider, mvcContextFactory, siteMapNodeChildStateFactory, localizationService, urlPath)
        {
            DataByExtensionKey = new Dictionary<string, object>();
        }

        public IDictionary<string, object> DataByExtensionKey { get; private set; }
    }
}