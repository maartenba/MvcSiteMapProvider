// -----------------------------------------------------------------------
// <copyright file="RouteValueCollection.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MvcSiteMapProvider.Core.Collections;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RouteValueCollection
        : ObservableDictionary<string, object>
    {


        // TODO: Use this to replace the "Node matches route" method...?
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
