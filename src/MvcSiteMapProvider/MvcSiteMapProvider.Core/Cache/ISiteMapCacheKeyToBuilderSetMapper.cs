using System;
namespace MvcSiteMapProvider.Core.Cache
{
    public interface ISiteMapCacheKeyToBuilderSetMapper
    {
        string GetBuilderSetName(string cacheKey);
    }
}
