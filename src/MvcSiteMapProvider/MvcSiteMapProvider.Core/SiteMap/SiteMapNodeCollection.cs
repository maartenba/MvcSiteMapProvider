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
    public class SiteMapNodeCollection
        : LockableList<ISiteMapNode>, ISiteMapNodeCollection
    {
        public SiteMapNodeCollection(
            ISiteMap siteMap
            )
            : base(siteMap)
        {
        }

    }
}
