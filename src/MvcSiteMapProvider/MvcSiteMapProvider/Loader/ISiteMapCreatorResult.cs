using System;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Loader
{
    /// <summary>
    /// Contract for container to pass the results of the CreateSiteMap() method of 
    /// <see cref="T:MvcSiteMapProvider.Loader.ISiteMapCreator"/>.
    /// </summary>
    public interface ISiteMapCreatorResult
    {
        ISiteMap SiteMap { get; }
        ICacheDetails CacheDetails { get; }
    }
}
