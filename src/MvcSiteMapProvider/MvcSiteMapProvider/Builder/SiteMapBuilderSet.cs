using System;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Provides a named set of services that can be used to build a <see cref="T:MvcSiteMapProvider.ISiteMap"/>.
    /// </summary>
    public class SiteMapBuilderSet
        : ISiteMapBuilderSet
    {
        public SiteMapBuilderSet(
            string instanceName,
            string cacheDetailsName,
            ISiteMapBuilder siteMapBuilder,
            ICacheDetailsStrategy cacheDetailsStrategy
            )
        {
            if (string.IsNullOrEmpty(instanceName))
                throw new ArgumentNullException("instanceName");
            if (string.IsNullOrEmpty(cacheDetailsName))
                throw new ArgumentNullException("cacheDetailsName");
            if (siteMapBuilder == null)
                throw new ArgumentNullException("siteMapBuilder");
            if (cacheDetailsStrategy == null)
                throw new ArgumentNullException("cacheDetailsStrategy");

            this.instanceName = instanceName;
            this.cacheDetailsName = cacheDetailsName;
            this.siteMapBuilder = siteMapBuilder;
            this.cacheDetailsStrategy = cacheDetailsStrategy;
        }

        protected readonly string instanceName;
        protected readonly string cacheDetailsName;
        protected readonly ISiteMapBuilder siteMapBuilder;
        protected readonly ICacheDetailsStrategy cacheDetailsStrategy;


        #region ISiteMapBuilderSet<CacheDependency> Members

        public virtual ISiteMapBuilder Builder
        {
            get { return this.siteMapBuilder; }
        }

        public virtual ICacheDetails CacheDetails
        {
            get { return this.cacheDetailsStrategy.GetCacheDetails(this.cacheDetailsName); }
        }

        public virtual bool AppliesTo(string builderSetName)
        {
            return this.instanceName.Equals(builderSetName, StringComparison.InvariantCulture);
        }

        #endregion
    }
}
