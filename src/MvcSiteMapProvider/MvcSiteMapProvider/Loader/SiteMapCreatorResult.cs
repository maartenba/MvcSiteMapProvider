using System;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Loader
{
    /// <summary>
    /// Container to pass the results of the CreateSiteMap() method of 
    /// <see cref="T:MvcSiteMapProvider.Loader.ISiteMapCreator"/>.
    /// </summary>
    public class SiteMapCreatorResult 
        : ISiteMapCreatorResult
    {
        public SiteMapCreatorResult(
            ISiteMap siteMap,
            ICacheDetails cacheDetails
            )
        {
            if (siteMap == null)
                throw new ArgumentNullException("siteMap");
            if (cacheDetails == null)
                throw new ArgumentNullException("cacheDetails");

            this.SiteMap = siteMap;
            this.CacheDetails = cacheDetails;
        }

        public virtual ISiteMap SiteMap { get; protected set; }
        public virtual ICacheDetails CacheDetails { get; protected set; }
    }
}
