using System;
using System.Collections.Generic;
using System.Linq;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Tracks all of the registered instances of <see cref="T:MvcSiteMapProvider.Caching.ICacheDetails"/> and 
    /// allows the caller to get a specific named instance of this interface at runtime.
    /// </summary>
    public class CacheDetailsStrategy : MvcSiteMapProvider.Caching.ICacheDetailsStrategy
    {
        public CacheDetailsStrategy(
            ICacheDetails[] cacheDetailsArray
            )
        {
            if (cacheDetailsArray == null)
                throw new ArgumentNullException("cacheDetailsArray");
            this.cacheDetailsArray = cacheDetailsArray;
        }

        protected readonly ICacheDetails[] cacheDetailsArray;

        public virtual ICacheDetails GetCacheDetails(string cacheDetailsName)
        {
            var cacheDetails = cacheDetailsArray.FirstOrDefault(x => x.AppliesTo(cacheDetailsName));
            if (cacheDetails == null)
            {
                throw new MvcSiteMapException(String.Format(Resources.Messages.NamedCacheDetailsNotFound, cacheDetailsName));
            }
            return cacheDetails;
        }
    }
}
