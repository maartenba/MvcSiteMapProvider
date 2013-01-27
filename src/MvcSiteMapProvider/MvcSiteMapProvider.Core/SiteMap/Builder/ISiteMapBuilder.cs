// -----------------------------------------------------------------------
// <copyright file="ISiteMapLoader.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using MvcSiteMapProvider.Core;

namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapBuilder
    {
        ISiteMapNode BuildSiteMap(ISiteMap siteMap);
    }
}
