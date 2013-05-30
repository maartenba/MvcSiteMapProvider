namespace VideoSitemap
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using MvcSiteMapProvider;
    using MvcSiteMapProvider.Loader;
    using MvcSiteMapProvider.Web;
    using MvcSiteMapProvider.Web.Mvc;
    using SiteMapNode;

    public class VideoSitemapResultFactory : IXmlSiteMapResultFactory
    {
        private readonly ISiteMapLoader _siteMapLoader;
        private readonly IUrlPath _urlPath;

        public VideoSitemapResultFactory(ISiteMapLoader siteMapLoader, IUrlPath urlPath)
        {
            _siteMapLoader = siteMapLoader;
            _urlPath = urlPath;
        }

        protected virtual int DefaultPage
        {
            get { return 0; }
        }

        protected virtual ISiteMapNode DefaultRootNode
        {
            get { return _siteMapLoader.GetSiteMap().RootNode; }
        }

        protected virtual string DefaultSiteMapUrlTemplate
        {
            get { return "sitemap-{page}.xml"; }
        }

        protected virtual string DefaultBaseUrl
        {
            get { return _urlPath.ResolveServerUrl("~/", false); }
        }

        protected virtual IEnumerable<string> DefaultSiteMapCacheKeys
        {
            get { return new List<string>(); }
        }

        public ActionResult Create()
        {
            return new VideoXmlSitemapResult(DefaultPage, DefaultRootNode, DefaultSiteMapCacheKeys, DefaultBaseUrl, DefaultSiteMapUrlTemplate, _siteMapLoader);
        }

        public ActionResult Create(int page)
        {
            return new VideoXmlSitemapResult(page, DefaultRootNode, DefaultSiteMapCacheKeys, DefaultBaseUrl, DefaultSiteMapUrlTemplate, _siteMapLoader);
        }

        public ActionResult Create(IEnumerable<string> siteMapCacheKeys)
        {
            return new VideoXmlSitemapResult(DefaultPage, DefaultRootNode, siteMapCacheKeys, DefaultBaseUrl, DefaultSiteMapUrlTemplate, _siteMapLoader);
        }

        public ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys)
        {
            return new VideoXmlSitemapResult(page, DefaultRootNode, siteMapCacheKeys, DefaultBaseUrl, DefaultSiteMapUrlTemplate, _siteMapLoader);
        }

        public ActionResult Create(IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate)
        {
            return new VideoXmlSitemapResult(DefaultPage, DefaultRootNode, siteMapCacheKeys, baseUrl, siteMapUrlTemplate, _siteMapLoader);
        }

        public ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate)
        {
            return new VideoXmlSitemapResult(page, DefaultRootNode, siteMapCacheKeys, baseUrl, siteMapUrlTemplate, _siteMapLoader);
        }

        public ActionResult Create(ISiteMapNode rootNode)
        {
            return new VideoXmlSitemapResult(DefaultPage, rootNode, DefaultSiteMapCacheKeys, DefaultBaseUrl, DefaultSiteMapUrlTemplate, _siteMapLoader);
        }

        public ActionResult Create(int page, ISiteMapNode rootNode)
        {
            return new VideoXmlSitemapResult(page, rootNode, DefaultSiteMapCacheKeys, DefaultBaseUrl, DefaultSiteMapUrlTemplate, _siteMapLoader);
        }

        public ActionResult Create(ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate)
        {
            return new VideoXmlSitemapResult(DefaultPage, rootNode, DefaultSiteMapCacheKeys, baseUrl, siteMapUrlTemplate, _siteMapLoader);
        }

        public ActionResult Create(int page, ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate)
        {
            return new VideoXmlSitemapResult(page, rootNode, DefaultSiteMapCacheKeys, baseUrl, siteMapUrlTemplate, _siteMapLoader);
        }
    }
}