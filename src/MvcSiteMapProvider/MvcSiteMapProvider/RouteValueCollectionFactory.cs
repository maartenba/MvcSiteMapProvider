using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.RouteValueCollection"/>
    /// at runtime.
    /// </summary>
    public class RouteValueCollectionFactory
        : IRouteValueCollectionFactory
    {
        #region IRouteValueCollectionFactory Members

        public virtual IRouteValueCollection Create(ISiteMap siteMap)
        {
            return new RouteValueCollection(siteMap);
        }

        #endregion
    }
}
