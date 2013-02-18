using System;
using System.Web.Caching;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contains methods to abstract the creation of <see cref="T:System.Web.Caching.CacheDependency"/> 
    /// and <see cref="T:System.Web.Caching.AggregateCacheDependency"/> to allow mocks to be injected.
    /// </summary>
    public class CacheDependencyFactory
        : ICacheDependencyFactory
    {
        #region ICacheDependencyFactory Members

        public CacheDependency CreateFileDependency(string absoluteFilePath)
        {
            return new CacheDependency(absoluteFilePath);
        }

        public CacheDependency CreateAggregateDependency()
        {
            return new AggregateCacheDependency();
        }

        #endregion
    }
}
