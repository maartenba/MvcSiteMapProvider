using System;
using System.Web.Mvc;
using System.Collections.Generic;
using MvcSiteMapProvider.Loader;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Web.Mvc.XmlSiteMapResult"/>
    /// at runtime.
    /// </summary>
    public class XmlSiteMapResultFactory
        : IXmlSiteMapResultFactory
    {
        public XmlSiteMapResultFactory(
            ISiteMapLoader siteMapLoader,
            IUrlPath urlPath
            )
        {
            if (siteMapLoader == null)
                throw new ArgumentNullException("siteMapLoader");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            this.siteMapLoader = siteMapLoader;
            this.urlPath = urlPath;
        }

        protected readonly ISiteMapLoader siteMapLoader;
        protected readonly IUrlPath urlPath;

        

        #region IXmlSiteMapResultFactory Members

        public virtual ActionResult Create()
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                this.DefaultRootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader);
        }

        public ActionResult Create(int page)
        {
            return new XmlSiteMapResult(
                page,
                this.DefaultRootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader);
        }

        public virtual ActionResult Create(IEnumerable<string> siteMapCacheKeys)
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                this.DefaultRootNode,
                siteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader);
        }

        public ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys)
        {
            return new XmlSiteMapResult(
                page,
                this.DefaultRootNode,
                siteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader);
        }

        public virtual ActionResult Create(IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                this.DefaultRootNode,
                siteMapCacheKeys,
                baseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader);
        }

        public ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                page,
                this.DefaultRootNode,
                siteMapCacheKeys,
                baseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader);
        }

        public virtual ActionResult Create(ISiteMapNode rootNode)
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                rootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader);
        }

        public ActionResult Create(int page, ISiteMapNode rootNode)
        {
            return new XmlSiteMapResult(
                page,
                rootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader);
        }

        public virtual ActionResult Create(ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                rootNode,
                this.DefaultSiteMapCacheKeys,
                baseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader);
        }

        public ActionResult Create(int page, ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                page,
                rootNode,
                this.DefaultSiteMapCacheKeys,
                baseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader);
        }

        #endregion

        protected virtual int DefaultPage
        {
            get { return 0; }
        }

        protected virtual ISiteMapNode DefaultRootNode
        {
            get { return siteMapLoader.GetSiteMap().RootNode; }
        }

        protected virtual string DefaultSiteMapUrlTemplate
        {
            get { return "sitemap-{page}.xml"; }
        }

        protected virtual string DefaultBaseUrl
        {
            get { return urlPath.ResolveServerUrl("~/", false); }
        }

        protected virtual IEnumerable<string> DefaultSiteMapCacheKeys
        {
            get { return new List<string>(); }
        }

    }
}
