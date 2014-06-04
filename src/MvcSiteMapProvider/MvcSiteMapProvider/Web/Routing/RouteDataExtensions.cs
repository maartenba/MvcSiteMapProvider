using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace MvcSiteMapProvider.Web.Routing
{
    public static class RouteDataExtensions
    {
        /// <summary>
        /// Retrieves the value with the specified identifier. Unlike GetRequiredString, this 
        /// method does not throw an exception if the value does not exist or is null.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <param name="valueName">The key of the value to retrieve.</param>
        /// <returns>The value with the specified key, or an empty string if not found.</returns>
        public static string GetOptionalString(this RouteData routeData, string valueName)
        {
            object value;
            if (routeData.Values.TryGetValue(valueName, out value))
            {
                return value as string;
            }

            if (routeData.Values.ContainsKey("MS_DirectRouteMatches"))
            {
                if (((IEnumerable<RouteData>)routeData.Values["MS_DirectRouteMatches"]).First().Values.TryGetValue(valueName, out value))
                {
                    return value as string;
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// Retrieves the area name.
        /// </summary>
        /// <param name="routeData">The route data.</param>
        /// <returns>The area name, or an empty string if not found.</returns>
        public static string GetAreaName(this RouteData routeData)
        {
            object value;
            if (routeData.DataTokens.TryGetValue("area", out value))
            {
                return value as string;
            }

            if (routeData.Values.ContainsKey("MS_DirectRouteMatches"))
            {
                if (((IEnumerable<RouteData>)routeData.Values["MS_DirectRouteMatches"]).First().DataTokens.TryGetValue("area", out value))
                {
                    return value as string;
                }
            }

            return string.Empty;
        }
    }
}
