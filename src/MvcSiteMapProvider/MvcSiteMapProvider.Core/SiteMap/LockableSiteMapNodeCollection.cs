using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Core.Collections;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class LockableSiteMapNodeCollection
        : LockableList<ISiteMapNode>, ISiteMapNodeCollection
    {
        public LockableSiteMapNodeCollection(
            ISiteMap siteMap
            )
            : base(siteMap)
        {
        }

    }
}
