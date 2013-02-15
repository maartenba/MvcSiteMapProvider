using System;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapNodeCollectionFactory
    {
        ISiteMapNodeCollection Create();
        ISiteMapNodeCollection CreateLockable(ISiteMap siteMap);
        ISiteMapNodeCollection CreateReadOnly(ISiteMapNodeCollection siteMapNodeCollection);
        ISiteMapNodeCollection CreateEmptyReadOnly();
    }
}
