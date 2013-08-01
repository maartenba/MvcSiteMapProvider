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
                    if (!this.MatchesRouteValue(actionParameters, pair.Key, pair.Value))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        protected virtual bool MatchesRouteValue(IEnumerable<string> actionParameters, string key, object value)
        {
            if (this.ValueExists(key))
            {
                if (this.MatchesValue(key, value) || this.MatchesActionMethodParameter(actionParameters, key))
                {
                    return true;
                }
            }
            else
            {
                if (this.IsEmptyValue(value))
                {
                    return true;
                }
            }
            return false;
        }

        protected virtual bool MatchesActionMethodParameter(IEnumerable<string> actionParameters, string key)
        {
            return actionParameters.Contains(key, StringComparer.InvariantCultureIgnoreCase);
        }

        protected virtual bool MatchesValue(string key, object value)
        {
            return this[key].ToString().Equals(value.ToString(), StringComparison.InvariantCultureIgnoreCase);
        }

        protected virtual bool IsEmptyValue(object value)
        {
            return value == null ||
                string.IsNullOrEmpty(value.ToString()) || 
                value == UrlParameter.Optional;
        }

        protected virtual bool ValueExists(string key)
        {
            return this.ContainsKey(key) && 
                this[key] != null && 
                !string.IsNullOrEmpty(this[key].ToString());
        }
    }
}
