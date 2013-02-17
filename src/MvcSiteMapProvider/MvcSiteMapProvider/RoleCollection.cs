using System;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Collections;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RoleCollection
        : LockableList<string>, IRoleCollection
    {
        public RoleCollection(
            ISiteMap siteMap
            )
            : base(siteMap)
        {
        }
    }
}
