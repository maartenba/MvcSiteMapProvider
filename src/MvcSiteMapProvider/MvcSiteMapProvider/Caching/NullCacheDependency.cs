using System;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// An <see cref="T:MvcSiteMapProvider.Caching.ICacheDependency"/> implementation that can be used to indicate
    /// there are no cache dependencies.
    /// </summary>
    public class NullCacheDependency
        : ICacheDependency
    {
        #region ICacheDependency Members

        public object Dependency
        {
            get { return null; }
        }

        #endregion
    }
}
