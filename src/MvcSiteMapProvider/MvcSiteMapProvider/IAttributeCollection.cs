using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for the specialized collection for dealing with custom site map attributes.
    /// </summary>
    public interface IAttributeCollection
        : IDictionary<string, string>
    {
        bool MatchesRoute(IEnumerable<string> actionParameters, IDictionary<string, object> routeValues);
        void CopyTo(IDictionary<string, string> destination);
    }
}
