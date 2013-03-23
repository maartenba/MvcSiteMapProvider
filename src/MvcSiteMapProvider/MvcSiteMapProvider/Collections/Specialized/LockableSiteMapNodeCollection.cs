using System;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Provides the means to make the <see cref="T:MvcSiteMapProvider.ISiteMapNodeCollection"/> instance read-only so it cannot be 
    /// inadvertently altered while it is in the cache.
    /// </summary>
    public class LockableSiteMapNodeCollection
        : LockableList<ISiteMapNode>, ISiteMapNodeCollection
    {
        public LockableSiteMapNodeCollection(
            ISiteMap siteMap
            )
            : base(siteMap)
        {
        }

    }
}
