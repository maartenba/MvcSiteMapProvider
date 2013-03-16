using System;
using System.Web.Caching;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// This class wraps the <see cref="T:System.Web.Caching.Cache"/> object to allow type-safe
    /// interaction when managing cached <see cref="T:MvcSiteMapProvider.ISiteMap"/> instances.
    /// </summary>
    public class AspNetSiteMapCache
        : AspNetMicroCache<ISiteMap>, ISiteMapCache
    {
        public AspNetSiteMapCache(
            IMvcContextFactory mvcContextFactory
            )
            : base(mvcContextFactory)
        {
        }
    }
}
