using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// Contract of specialized dictionary for providing business logic that manages
    /// the behavior of the route values.
    /// </summary>
    public interface IRouteValueDictionary
        : IDictionary<string, object>
    {
        bool MatchesRoute(IEnumerable<string> actionParameters, IDictionary<string, object> routeValues);
        void CopyTo(IDictionary<string, object> destination);
    }
}
