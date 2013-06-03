namespace MvcSiteMapProvider
{
    using System.Collections.Generic;
    using Globalization;
    using Web;
    using Web.Mvc;

    public class ExtensibleSiteMapNode : SiteMapNode, IExtensibleSiteMapNode
    {
        public ExtensibleSiteMapNode(ISiteMap siteMap, string key, bool isDynamic, ISiteMapNodePluginProvider pluginProvider, IMvcContextFactory mvcContextFactory, ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory, ILocalizationService localizationService, IUrlPath urlPath) 
            : base(siteMap, key, isDynamic, pluginProvider, mvcContextFactory, siteMapNodeChildStateFactory, localizationService, urlPath)
        {
            DataByExtensionKey = new Dictionary<string, object>();
        }

        public IDictionary<string, object> DataByExtensionKey { get; private set; }
    }
}