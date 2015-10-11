using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
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
