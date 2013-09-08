using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for a set of services for creating SiteMap nodes, including dynamic nodes.
    /// </summary>
    public interface ISiteMapNodeBuilderService
    {
        ISiteMapNodeCreationService SiteMapNodeCreationService { get; }
        IDynamicNodeParentMapBuilder DynamicNodeParentMapBuilder { get; }
    }
}
