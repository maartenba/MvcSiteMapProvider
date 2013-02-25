using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Specialized dictionary for providing business logic that manages
    /// the behavior of the route values.
    /// </summary>
    public class RouteValueCollection
        : RequestCacheableDictionary<string, object>, IRouteValueCollection
    {
        public RouteValueCollection(
            ISiteMap siteMap,
            IRequestCache requestCache
            ) : base(siteMap, requestCache)
        {
        }

        protected override string GetCacheKey()
        {
            return "__ROUTE_VALUE_COLLECTION_" + this.instanceId.ToString();
        }

        public virtual bool MatchesRoute(IDictionary<string, object> routeValues)
        {
            var routeKeys = this.Keys;

            foreach (var pair in routeValues)
            {
                if (routeKeys.Contains(pair.Key) && !this[pair.Key].ToString().Equals(pair.Value.ToString(), StringComparison.InvariantCultureIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
