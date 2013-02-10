using System;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapNodeCollectionFactory
    {
        ISiteMapNodeCollection Create(ISiteMap siteMap);
        ISiteMapNodeCollection CreateReadOnly(ISiteMapNodeCollection siteMapNodeCollection);
        ISiteMapNodeCollection CreateEmptyReadOnly(ISiteMap siteMap);
    }
}
