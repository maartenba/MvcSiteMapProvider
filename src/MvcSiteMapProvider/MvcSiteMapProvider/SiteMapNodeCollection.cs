using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MvcSiteMapProvider.Collections;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// A specialized collection that manages relationships between <see cref="T:MvcSiteMapNode.ISiteMapNode"/>
    /// instances.
    /// </summary>
    public class SiteMapNodeCollection
        : List<ISiteMapNode>, ISiteMapNodeCollection
    {
    }
}
