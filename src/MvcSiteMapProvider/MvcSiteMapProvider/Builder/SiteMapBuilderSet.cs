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
            string name,
            ISiteMapBuilder siteMapBuilder,
            ICacheDetails cacheDetails
            )
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (siteMapBuilder == null)
                throw new ArgumentNullException("siteMapBuilder");
            if (cacheDetails == null)
                throw new ArgumentNullException("cacheDetails");

            this.name = name;
            this.siteMapBuilder = siteMapBuilder;
            this.cacheDetails = cacheDetails;
        }

        protected readonly string name;
        protected readonly ISiteMapBuilder siteMapBuilder;
        protected readonly ICacheDetails cacheDetails;


        #region ISiteMapBuilderSet<CacheDependency> Members

        public virtual string Name
        {
            get { return this.name; }
        }

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
            return this.name.Equals(builderSetName);
        }

        #endregion
    }
}
