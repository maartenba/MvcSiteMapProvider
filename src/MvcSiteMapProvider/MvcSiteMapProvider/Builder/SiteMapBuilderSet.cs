using MvcSiteMapProvider.Caching;
using System;

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
           bool useTitleIfDescriptionNotProvided,
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
            this.useTitleIfDescriptionNotProvided = useTitleIfDescriptionNotProvided;
            this.siteMapBuilder = siteMapBuilder;
            this.cacheDetails = cacheDetails;
        }

        /// <summary>
        /// ctor for backward compatibility, 
        /// visibilityAffectsDescendants parameter defaults to true
        /// useTitleIfDescriptionNotProvided parameter defaults to true
        /// </summary>
        [Obsolete("Use the overload ctor(string, bool, bool, bool, bool, ISiteMapBuilder, ICacheDetails) instead.")]
        public SiteMapBuilderSet(
            string instanceName,
            bool securityTrimmingEnabled,
            bool enableLocalization,
            ISiteMapBuilder siteMapBuilder,
            ICacheDetails cacheDetails
            ) 
            : this(
                instanceName,
                securityTrimmingEnabled,
                enableLocalization,
                true,
                true,
                siteMapBuilder,
                cacheDetails
            ) 
        { 
        }

        protected readonly string instanceName;
        protected readonly bool securityTrimmingEnabled;
        protected readonly bool enableLocalization;
        protected readonly bool visibilityAffectsDescendants;
        protected readonly bool useTitleIfDescriptionNotProvided;
        protected readonly ISiteMapBuilder siteMapBuilder;
        protected readonly ICacheDetails cacheDetails;

        #region ISiteMapBuilderSet Members

        public virtual ISiteMapBuilder Builder
        {
            get { return this.siteMapBuilder; }
        }

        public virtual ICacheDetails CacheDetails
        {
            get { return this.cacheDetails; }
        }

        public virtual string SiteMapCacheKey { get; set; }

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

        public virtual bool UseTitleIfDescriptionNotProvided
        {
            get { return this.useTitleIfDescriptionNotProvided; }
        }

        public virtual bool AppliesTo(string builderSetName)
        {
            return this.instanceName.Equals(builderSetName, StringComparison.Ordinal);
        }

        #endregion
    }
}
