using MvcSiteMapProvider.Builder;
using System.Collections.Generic;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for a provider that creates a list of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeToParentRelation"/>
    /// instances that can be processed later and put into a <see cref="T:MvcSiteMapProvider.ISiteMap"/>.
    /// </summary>
    public interface ISiteMapNodeProvider
    {
        IEnumerable<ISiteMapNodeToParentRelation> GetSiteMapNodes(ISiteMapNodeHelper helper);
    }
}
