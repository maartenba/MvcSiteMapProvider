using System;
using System.Web.Mvc;
using System.Collections.Generic;
using MvcSiteMapProvider.Globalization;
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
            IUrlPath urlPath,
            ICultureContextFactory cultureContextFactory
            )
        {
            if (siteMapLoader == null)
                throw new ArgumentNullException("siteMapLoader");
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");
            if (cultureContextFactory == null)
                throw new ArgumentNullException("cultureContextFactory");

            this.siteMapLoader = siteMapLoader;
            this.urlPath = urlPath;
            this.cultureContextFactory = cultureContextFactory;
        }

        protected readonly ISiteMapLoader siteMapLoader;
        protected readonly IUrlPath urlPath;
        protected readonly ICultureContextFactory cultureContextFactory;

        #region IXmlSiteMapResultFactory Members

        public virtual ActionResult Create(int page)
        {
            return new XmlSiteMapResult(
                page,
                this.DefaultRootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        public virtual ActionResult Create(int page, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                page,
                this.DefaultRootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }
        
        public virtual ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys)
        {
            return new XmlSiteMapResult(
                page,
                this.DefaultRootNode,
                siteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        public virtual ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                page,
                this.DefaultRootNode,
                siteMapCacheKeys,
                this.DefaultBaseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        public virtual ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                page,
                this.DefaultRootNode,
                siteMapCacheKeys,
                baseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        public virtual ActionResult Create(int page, ISiteMapNode rootNode)
        {
            return new XmlSiteMapResult(
                page,
                rootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        public virtual ActionResult Create(int page, ISiteMapNode rootNode, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                page,
                rootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        public virtual ActionResult Create(int page, ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                page,
                rootNode,
                this.DefaultSiteMapCacheKeys,
                baseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page) instead. This overload will be removed in version 5.")]
        public virtual ActionResult Create()
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                this.DefaultRootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page, IEnumerable<string> siteMapCacheKeys) instead. This overload will be removed in version 5.")]
        public virtual ActionResult Create(IEnumerable<string> siteMapCacheKeys)
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                this.DefaultRootNode,
                siteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate) instead. This overload will be removed in version 5.")]
        public virtual ActionResult Create(IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                this.DefaultRootNode,
                siteMapCacheKeys,
                baseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page, ISiteMapNode rootNode) instead. This overload will be removed in version 5.")]
        public virtual ActionResult Create(ISiteMapNode rootNode)
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                rootNode,
                this.DefaultSiteMapCacheKeys,
                this.DefaultBaseUrl,
                this.DefaultSiteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
        }

        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page, IEnumerable<string> siteMapCacheKeys) instead. This overload will be removed in version 5.")]
        public virtual ActionResult Create(ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate)
        {
            return new XmlSiteMapResult(
                this.DefaultPage,
                rootNode,
                this.DefaultSiteMapCacheKeys,
                baseUrl,
                siteMapUrlTemplate,
                this.siteMapLoader,
                this.urlPath,
                this.cultureContextFactory);
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
            get { return this.urlPath.ResolveUrl("/", Uri.UriSchemeHttp); }
        }

        protected virtual IEnumerable<string> DefaultSiteMapCacheKeys
        {
            get { return new List<string>(); }
        }
    }
}