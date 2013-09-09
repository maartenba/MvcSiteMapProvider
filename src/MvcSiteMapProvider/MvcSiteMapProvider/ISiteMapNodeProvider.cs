using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Builder;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for a provider that creates a list of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeParentMap"/>
    /// instances that can be processed later and put into a <see cref="T:MvcSiteMapProvider.ISiteMap"/>.
    /// </summary>
    public interface ISiteMapNodeProvider
    {
        IEnumerable<ISiteMapNodeParentMap> GetSiteMapNodes(ISiteMapNodeHelper helper);
    }
}
