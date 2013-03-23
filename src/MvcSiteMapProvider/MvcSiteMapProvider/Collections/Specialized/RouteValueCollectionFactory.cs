﻿using System;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Collections.Specialized
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.RouteValueCollection"/>
    /// at runtime.
    /// </summary>
    public class RouteValueCollectionFactory
        : IRouteValueCollectionFactory
    {
        public RouteValueCollectionFactory(
            IRequestCache requestCache
            )
        {
            if (requestCache == null)
                throw new ArgumentNullException("requestCache");

            this.requestCache = requestCache;
        }

        protected readonly IRequestCache requestCache;

        #region IRouteValueCollectionFactory Members

        public virtual IRouteValueCollection Create(ISiteMap siteMap)
        {
            return new RouteValueCollection(siteMap, this.requestCache);
        }

        #endregion
    }
}
