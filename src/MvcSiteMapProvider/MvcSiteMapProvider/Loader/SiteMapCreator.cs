using System;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Loader
{
    /// <summary>
    /// Builds a specific <see cref="T:MvcSiteMapProvider.ISiteMap"/> instance based on a cache key.
    /// </summary>
    public class SiteMapCreator 
        : ISiteMapCreator
    {
        public SiteMapCreator(
            ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper,
            ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy,
            ISiteMapFactory siteMapFactory
            )
        {
            if (siteMapCacheKeyToBuilderSetMapper == null)
                throw new ArgumentNullException("siteMapCacheKeyToBuilderSetMapper");
            if (siteMapBuilderSetStrategy == null)
                throw new ArgumentNullException("siteMapBuilderSetStrategy");
            if (siteMapFactory == null)
                throw new ArgumentNullException("siteMapFactory");
            
            this.siteMapCacheKeyToBuilderSetMapper = siteMapCacheKeyToBuilderSetMapper;
            this.siteMapBuilderSetStrategy = siteMapBuilderSetStrategy;
            this.siteMapFactory = siteMapFactory;
        }

        protected readonly ISiteMapCacheKeyToBuilderSetMapper siteMapCacheKeyToBuilderSetMapper;
        protected readonly ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy;
        protected readonly ISiteMapFactory siteMapFactory;

        #region ISiteMapCreator Members

        public virtual ISiteMap CreateSiteMap(string siteMapCacheKey)
        {
            if (string.IsNullOrEmpty(siteMapCacheKey))
            {
                throw new ArgumentNullException("siteMapCacheKey");
            }

            var builderSet = this.GetBuilderSet(siteMapCacheKey);
            var siteMap = siteMapFactory.Create(builderSet.Builder, builderSet);
            siteMap.BuildSiteMap();

            return siteMap;
        }

        public virtual ICacheDetails GetCacheDetails(string siteMapCacheKey)
        {
            var builderSet = this.GetBuilderSet(siteMapCacheKey);
            return builderSet.CacheDetails;
        }

        #endregion

        protected virtual ISiteMapBuilderSet GetBuilderSet(string siteMapCacheKey)
        {
            var builderSetName = siteMapCacheKeyToBuilderSetMapper.GetBuilderSetName(siteMapCacheKey);
            var builderSet = siteMapBuilderSetStrategy.GetBuilderSet(builderSetName);
            builderSet.SiteMapCacheKey = siteMapCacheKey;
            return builderSet;
        }
    }
}
