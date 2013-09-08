using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for a class that builds a hierarchy of nodes and adds them to the SiteMap.
    /// </summary>
    public interface ISiteMapHierarchyBuilder
    {
        IEnumerable<ISiteMapNodeParentMap> BuildHierarchy(ISiteMap siteMap, IEnumerable<ISiteMapNodeParentMap> nodes);
    }
}
