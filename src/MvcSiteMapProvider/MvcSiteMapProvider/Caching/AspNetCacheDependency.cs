using System;
using System.Collections.Generic;
using System.Web.Caching;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// A wrapper class to pass a concrete instance of <see cref="System.Web.Caching.CacheDependency"/> without creating
    /// a dependency on the System.Web library.
    /// </summary>
    public class AspNetCacheDependency
        : ICacheDependency
    {
        public AspNetCacheDependency(
            CacheDependency cacheDependency
            )
        {
            if (cacheDependency == null)
                throw new ArgumentNullException("cacheDependency");

            this.cacheDependency = cacheDependency;
        }

        protected readonly CacheDependency cacheDependency;

        #region ICacheDependency Members

        public object Dependency
        {
            get { return this.cacheDependency; }
        }

        #endregion
    }
}
