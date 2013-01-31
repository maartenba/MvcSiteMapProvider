// -----------------------------------------------------------------------
// <copyright file="ISiteMapFactory.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MvcSiteMapProvider.Core.SiteMap.Builder;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapFactory
    {
        ISiteMap Create(ISiteMapBuilder siteMapBuilder);
    }
}
