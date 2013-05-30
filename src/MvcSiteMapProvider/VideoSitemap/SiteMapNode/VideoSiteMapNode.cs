namespace VideoSitemap.SiteMapNode
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using MvcSiteMapProvider;
    using MvcSiteMapProvider.Globalization;
    using MvcSiteMapProvider.Web;
    using MvcSiteMapProvider.Web.Mvc;

    public class VideoSiteMapNode : SiteMapNode
    {
        public VideoSiteMapNode(ISiteMap siteMap, string key, bool isDynamic, ISiteMapNodePluginProvider pluginProvider, IMvcContextFactory mvcContextFactory, ISiteMapNodeChildStateFactory siteMapNodeChildStateFactory, ILocalizationService localizationService, IUrlPath urlPath) 
            : base(siteMap, key, isDynamic, pluginProvider, mvcContextFactory, siteMapNodeChildStateFactory, localizationService, urlPath)
        {
            VideoNodeInformation = new Collection<VideoNodeInformation>();
        }

        public ICollection<VideoNodeInformation> VideoNodeInformation { get; set; }
    }
}