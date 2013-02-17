using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Collections;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PreservedRouteParameterCollection
        : LockableList<string>, IPreservedRouteParameterCollection
    {
        public PreservedRouteParameterCollection(
            ISiteMap siteMap
            )
            : base(siteMap)
        {
        }
    }
}
