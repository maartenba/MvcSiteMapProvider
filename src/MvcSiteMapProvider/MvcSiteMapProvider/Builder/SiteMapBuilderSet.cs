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
            ISiteMapBuilder siteMapBuilder,
            ICacheDetails cacheDetails
            )
        {
            if (string.IsNullOrEmpty(instanceName))
                throw new ArgumentNullException("instanceName");
            if (siteMapBuilder == null)
                throw new ArgumentNullException("siteMapBuilder");
            if (cacheDetails == null)
                throw new ArgumentNullException("cacheDetails");

            this.instanceName = instanceName;
            this.siteMapBuilder = siteMapBuilder;
            this.cacheDetails = cacheDetails;
        }

        protected readonly string instanceName;
        protected readonly ISiteMapBuilder siteMapBuilder;
        protected readonly ICacheDetails cacheDetails;


        #region ISiteMapBuilderSet<CacheDependency> Members

        public virtual ISiteMapBuilder Builder
        {
            get { return this.siteMapBuilder; }
        }

        public virtual ICacheDetails CacheDetails
        {
            get { return this.cacheDetails; }
        }

        public virtual bool AppliesTo(string builderSetName)
        {
            return this.instanceName.Equals(builderSetName, StringComparison.InvariantCulture);
        }

        #endregion
    }
}
