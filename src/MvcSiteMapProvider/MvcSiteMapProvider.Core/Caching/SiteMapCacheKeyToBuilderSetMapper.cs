using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Core.Caching
{
    /// <summary>
    /// TODO: Update summary.
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
