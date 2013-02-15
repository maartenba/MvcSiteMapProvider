using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Core
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RouteValueCollectionFactory
        : IRouteValueCollectionFactory
    {

        #region IRouteValueCollectionFactory Members

        public IRouteValueCollection Create(ISiteMap siteMap)
        {
            return new RouteValueCollection(siteMap);
        }

        #endregion
    }
}
