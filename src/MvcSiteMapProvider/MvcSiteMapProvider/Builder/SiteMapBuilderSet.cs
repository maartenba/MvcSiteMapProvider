using System;
using System.Web.Caching;
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
            ICacheDependencyFactory cacheDependencyFactory
            )
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (siteMapBuilder == null)
                throw new ArgumentNullException("siteMapBuilder");
            if (cacheDependencyFactory == null)
                throw new ArgumentNullException("cacheDependencyFactory");

            this.name = name;
            this.siteMapBuilder = siteMapBuilder;
            this.cacheDependencyFactory = cacheDependencyFactory;
        }

        protected readonly string name;
        protected readonly ISiteMapBuilder siteMapBuilder;
        protected readonly ICacheDependencyFactory cacheDependencyFactory;


        #region ISiteMapBuilderSet<CacheDependency> Members

        public virtual string Name
        {
            get { return this.name; }
        }

        public virtual ISiteMapBuilder Builder
        {
            get { return this.siteMapBuilder; }
        }

        public virtual ICacheDependency CreateCacheDependency()
        {
            return this.cacheDependencyFactory.Create();
        }

        public virtual bool AppliesTo(string builderSetName)
        {
            return this.name.Equals(builderSetName);
        }

        #endregion
    }
}
