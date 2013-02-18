using System;
using System.Web.Caching;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class SiteMapBuilderSet
        : ISiteMapBuilderSet
    {
        public SiteMapBuilderSet(
            string name,
            ISiteMapBuilder siteMapBuilder,
            ICacheDependency cacheDependency
            )
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (siteMapBuilder == null)
                throw new ArgumentNullException("siteMapBuilder");
            if (cacheDependency == null)
                throw new ArgumentNullException("cacheDependency");

            this.name = name;
            this.siteMapBuilder = siteMapBuilder;
            this.cacheDependency = cacheDependency;
        }

        protected readonly string name;
        protected readonly ISiteMapBuilder siteMapBuilder;
        protected readonly ICacheDependency cacheDependency;


        #region ISiteMapBuilderSet<CacheDependency> Members

        public string Name
        {
            get { return this.name; }
        }

        public ISiteMapBuilder Builder
        {
            get { return this.siteMapBuilder; }
        }

        public ICacheDependency CacheDependency
        {
            get { return this.cacheDependency; }
        }

        public bool AppliesTo(string builderSetName)
        {
            return this.name.Equals(builderSetName);
        }

        #endregion
    }
}
