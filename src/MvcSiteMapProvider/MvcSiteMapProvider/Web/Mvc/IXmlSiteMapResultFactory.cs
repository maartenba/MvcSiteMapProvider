using System;
using System.Web.Mvc;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// IXmlSiteMapResultFactory interface
    /// </summary>
    public interface IXmlSiteMapResultFactory
    {
        ActionResult Create();
        ActionResult Create(int page);
        ActionResult Create(IEnumerable<string> siteMapCacheKeys);
        ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys);
        ActionResult Create(IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate);
        ActionResult Create(int page, IEnumerable<string> siteMapCacheKeys, string baseUrl, string siteMapUrlTemplate);
        ActionResult Create(ISiteMapNode rootNode);
        ActionResult Create(int page, ISiteMapNode rootNode);
        ActionResult Create(ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate);
        ActionResult Create(int page, ISiteMapNode rootNode, string baseUrl, string siteMapUrlTemplate);
    }
}
