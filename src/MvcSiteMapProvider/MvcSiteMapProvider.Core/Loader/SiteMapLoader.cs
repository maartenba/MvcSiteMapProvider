// -----------------------------------------------------------------------
// <copyright file="SiteMapLoader.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.Loader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using System.Threading;
    using MvcSiteMapProvider.Core.Cache;
    using MvcSiteMapProvider.Core.SiteMap;
    using MvcSiteMapProvider.Core.SiteMap.Builder;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapLoader 
        : ISiteMapLoader
    {
        public SiteMapLoader(
            TimeSpan slidingCacheExpiration,
            ISiteMapCache siteMapCache,
            ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator,
            ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy,
            ISiteMapFactory siteMapFactory
            )
        {
            if (slidingCacheExpiration == null)
                throw new ArgumentNullException("slidingCacheExpiration");
            if (siteMapCache == null)
                throw new ArgumentNullException("siteMapCache");
            if (siteMapCacheKeyGenerator == null)
                throw new ArgumentNullException("siteMapCacheKeyGenerator");
            if (siteMapBuilderSetStrategy == null)
                throw new ArgumentNullException("siteMapBuilderSetStrategy");
            if (siteMapFactory == null)
                throw new ArgumentNullException("siteMapFactory");

            this.slidingCacheExpiration = slidingCacheExpiration;
            this.siteMapCache = siteMapCache;
            this.siteMapCacheKeyGenerator = siteMapCacheKeyGenerator;
            this.siteMapBuilderSetStrategy = siteMapBuilderSetStrategy;
            this.siteMapFactory = siteMapFactory;
        }

        private readonly TimeSpan slidingCacheExpiration;
        private readonly ISiteMapCache siteMapCache;
        private readonly ISiteMapCacheKeyGenerator siteMapCacheKeyGenerator;
        private readonly ISiteMapBuilderSetStrategy siteMapBuilderSetStrategy;
        private readonly ISiteMapFactory siteMapFactory;

        protected readonly object synclock = new object();

        public ISiteMap GetSiteMap(string builderSetName)
        {
            var key = siteMapCacheKeyGenerator.GenerateKey(HttpContext.Current);
            return GetSiteMap(key, builderSetName);
        }

        public ISiteMap GetSiteMap(string siteMapKey, string builderSetName)
        {
            lock (this.synclock)
            {
                var siteMap = siteMapCache[siteMapKey];
                if (siteMap == null)
                {
                    // Build sitemap
                    var builder = siteMapBuilderSetStrategy.GetBuilder(builderSetName);
                    siteMap = siteMapFactory.Create(builder);
                    siteMap.BuildSiteMap();

                    siteMapCache.Insert(siteMapKey, siteMap, System.Web.Caching.Cache.NoAbsoluteExpiration, slidingCacheExpiration);
                }
                return siteMap;
            }
        }
    }
}
