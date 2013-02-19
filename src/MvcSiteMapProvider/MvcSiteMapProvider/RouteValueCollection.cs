using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Specialized dictionary for providing business logic that manages
    /// the behavior of the route values.
    /// </summary>
    public class RouteValueCollection
        : LockableDictionary<string, object>, IRouteValueCollection
    {
        public RouteValueCollection(
            ISiteMap siteMap
            ) : base(siteMap)
        {
        }

        public bool MatchesRoute(IDictionary<string, object> routeValues)
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
