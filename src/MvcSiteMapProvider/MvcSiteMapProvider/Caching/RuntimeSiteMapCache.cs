#if !NET35
using System;
using System.Runtime.Caching;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// This class wraps <see cref="T:System.Runtime.Caching.ObjectCache"/> to allow type-safe
    /// interaction when managing cached <see cref="T:MvcSiteMapProvider.ISiteMap"/> instances.
    /// </summary>
    public class RuntimeSiteMapCache
        : RuntimeMicroCache<ISiteMap>, ISiteMapCache
    {
        public RuntimeSiteMapCache(ObjectCache cache)
            : base(cache)
        {
        }
    }
}
#endif