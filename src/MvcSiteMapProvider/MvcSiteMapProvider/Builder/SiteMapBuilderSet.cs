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
           bool securityTrimmingEnabled,
           bool enableLocalization,
           bool visibilityAffectsDescendants,
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
            this.securityTrimmingEnabled = securityTrimmingEnabled;
            this.enableLocalization = enableLocalization;
            this.visibilityAffectsDescendants = visibilityAffectsDescendants;
            this.siteMapBuilder = siteMapBuilder;
            this.cacheDetails = cacheDetails;
        }

        /// <summary>
        /// ctor for backwards compatibility, 
        /// visibilityAffectsDescendants parameter defaults to false
        /// </summary>
        [Obsolete]
        public SiteMapBuilderSet(
            string instanceName,
            bool securityTrimmingEnabled,
            bool enableLocalization,
            ISiteMapBuilder siteMapBuilder,
            ICacheDetails cacheDetails
            ) :
            this(
            instanceName,
            securityTrimmingEnabled,
            enableLocalization,
            false,
            siteMapBuilder,
            cacheDetails) { }


        protected readonly string instanceName;
        protected readonly bool securityTrimmingEnabled;
        protected readonly bool enableLocalization;
        protected readonly bool visibilityAffectsDescendants;
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

        public virtual bool SecurityTrimmingEnabled
        {
            get { return this.securityTrimmingEnabled; }
        }

        public virtual bool EnableLocalization
        {
            get { return this.enableLocalization; }
        }

        public virtual bool VisibilityAffectsDescendants
        {
            get { return this.visibilityAffectsDescendants; }
        }

        public virtual bool AppliesTo(string builderSetName)
        {
            return this.instanceName.Equals(builderSetName, StringComparison.InvariantCulture);
        }

        #endregion
    }
}
