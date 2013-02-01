// -----------------------------------------------------------------------
// <copyright file="ISiteMapNodeFactory.cs" company="">
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
    /// An abstract factory interface for creating sitemap nodes at runtime.
    /// </summary>
    public interface ISiteMapNodeFactory
    {
        ISiteMapNode Create(ISiteMap siteMap, string key, string implicitResourceKey);
        ISiteMapNode CreateDynamic(ISiteMap siteMap, string key, string implicitResourceKey);
    }
}
