using System;
using MvcSiteMapProvider.Core.Builder;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapFactory
    {
        ISiteMap Create(ISiteMapBuilder siteMapBuilder);
    }
}
