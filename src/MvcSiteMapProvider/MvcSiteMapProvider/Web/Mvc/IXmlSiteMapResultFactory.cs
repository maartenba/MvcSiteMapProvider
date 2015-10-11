using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// IXmlSiteMapResultFactory interface
    /// </summary>
    public interface IXmlSiteMapResultFactory
    {
        ActionResult Create(int page);
        ActionResult Create(int page, string siteMapUrlTemplate);
        ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys);
        ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys, string siteMapUrlTemplate);
        ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate);
        ActionResult Create(int page, ISiteMapNode rootNode);
        ActionResult Create(int page, ISiteMapNode rootNode, string siteMapUrlTemplate);
        ActionResult Create(int page, ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate);

        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page) instead. This overload will be removed in version 5.")]
        ActionResult Create();
        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page, IEnumerable<string> siteMapCacheKeys) instead. This overload will be removed in version 5.")]
        ActionResult Create(IEnumerable<string> siteMapCacheKeys);
        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate) instead. This overload will be removed in version 5.")]
        ActionResult Create(IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate);
        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page, ISiteMapNode rootNode) instead. This overload will be removed in version 5.")]
        ActionResult Create(ISiteMapNode rootNode);
        [Obsolete("Overload is invalid for sitemaps with over 35,000 links. Use Create(int page, IEnumerable<string> siteMapCacheKeys) instead. This overload will be removed in version 5.")]
        ActionResult Create(ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate);
    }
}
