using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// The default mapper class that simply maps everything to the default <see cref="T:MvcSiteMapProvider.Builder.ISiteMapBulderSet"/>.
    /// This class can be inherited to provide mapping logic that gets its data from a configuration file or database. 
    /// </summary>
    public class SiteMapCacheKeyToBuilderSetMapper 
        : ISiteMapCacheKeyToBuilderSetMapper
    {
        public virtual string GetBuilderSetName(string cacheKey)
        {
            return "default";
        }
    }
}
