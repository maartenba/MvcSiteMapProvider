using System;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of 
    /// <see cref="T:MvcSiteMapProvider.Collections.Specialized.RouteValueDictionary"/>
    /// at runtime.
    /// </summary>
    public class RouteValueDictionaryFactory
        : IRouteValueDictionaryFactory
    {
        public RouteValueDictionaryFactory(
            IRequestCache requestCache
            )
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.requestCache = requestCache;
        }

        protected readonly IRequestCache requestCache;

        #region IRouteValueDictionaryFactory Members

        public virtual IRouteValueDictionary Create(ISiteMap siteMap)
        {
            return new RouteValueDictionary(siteMap, this.requestCache);
        }

        #endregion
    }
}
