using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Specialized dictionary for providing business logic that manages
    /// the behavior of the route values.
    /// </summary>
    public class RouteValueDictionary
        : RequestCacheableDictionary<string, object>, IRouteValueDictionary
    {
        public RouteValueDictionary(
            ISiteMap siteMap,
            IRequestCache requestCache
            ) : base(siteMap, requestCache)
        {
        }

        protected override string GetCacheKey()
        {
            return "__ROUTE_VALUE_COLLECTION_" + this.instanceId.ToString();
        }

        public virtual bool MatchesRoute(IEnumerable<string> actionParameters, IDictionary<string, object> routeValues)
        {
            if (routeValues.Count > 0)
            {
                foreach (var pair in routeValues)
                {
                    if (this.ContainsKey(pair.Key) && this[pair.Key] != null && !string.IsNullOrEmpty(this[pair.Key].ToString()))
                    {
                        if (this[pair.Key].ToString().ToLowerInvariant() == pair.Value.ToString().ToLowerInvariant())
                        {
                            continue;
                        }
                        else
                        {
                            // Is the current pair.Key a parameter on the action method?
                            if (!actionParameters.Contains(pair.Key, StringComparer.InvariantCultureIgnoreCase))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (pair.Value == null || string.IsNullOrEmpty(pair.Value.ToString()) || pair.Value == UrlParameter.Optional)
                        {
                            continue;
                        }
                        else if (pair.Key == "area")
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }
    }
}
