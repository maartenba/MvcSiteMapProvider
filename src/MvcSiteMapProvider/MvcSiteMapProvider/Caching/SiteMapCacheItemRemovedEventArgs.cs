using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapCacheItemRemovedEventArgs
        : EventArgs
    {
        public ISiteMap SiteMap { get; set; }
    }
}
