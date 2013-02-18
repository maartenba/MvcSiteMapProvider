using System;
using System.Web.Caching;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contains methods to abstract the creation of <see cref="T:System.Web.Caching.CacheDependency"/> 
    /// and <see cref="T:System.Web.Caching.AggregateCacheDependency"/> to allow mocks to be injected.
    /// </summary>
    public interface ICacheDependencyFactory
    {
        CacheDependency CreateFileDependency(string absoluteFilePath);
        CacheDependency CreateAggregateDependency();
    }
}
