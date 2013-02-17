using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IRouteValueCollection
        : IDictionary<string, object>
    {
        bool MatchesRoute(IDictionary<string, object> routeValues);
        void CopyTo(IDictionary<string, object> destination);
    }
}
