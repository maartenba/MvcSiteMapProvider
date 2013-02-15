using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IAttributeCollection
        : IDictionary<string, string>
    {
        bool MatchesRoute(IEnumerable<string> actionParameters, IDictionary<string, object> routeValues);
    }
}
