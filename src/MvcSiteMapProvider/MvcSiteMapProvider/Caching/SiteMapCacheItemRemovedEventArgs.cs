using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// A specialized <see cref="T:System.EventArgs"/> superclass that provides 
    /// access to the <see cref="T:MvcSiteMapProvider.ISiteMap"/> instance that was 
    /// removed from the cache.
    /// </summary>
    public class SiteMapCacheItemRemovedEventArgs
        : EventArgs
    {
        public ISiteMap SiteMap { get; set; }
    }
}
