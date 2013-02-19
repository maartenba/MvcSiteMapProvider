using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract of specialized string collection for providing business logic that manages
    /// the behavior of the route values.
    /// </summary>
    public interface IRouteValueCollection
        : IDictionary<string, object>
    {
        bool MatchesRoute(IDictionary<string, object> routeValues);
        void CopyTo(IDictionary<string, object> destination);
    }
}
