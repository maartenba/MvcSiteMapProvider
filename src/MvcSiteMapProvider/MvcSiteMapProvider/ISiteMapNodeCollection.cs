using System;
using System.Web.UI;
using System.Collections;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for specialized collection that manages relationships between <see cref="T:MvcSiteMapNode.ISiteMapNode"/>
    /// instances.
    /// </summary>
    public interface ISiteMapNodeCollection
        : IList<ISiteMapNode>, ICollection<ISiteMapNode>, IEnumerable<ISiteMapNode>
    {
        void AddRange(IEnumerable<ISiteMapNode> collection);
        void RemoveRange(int index, int count);
    }
}
