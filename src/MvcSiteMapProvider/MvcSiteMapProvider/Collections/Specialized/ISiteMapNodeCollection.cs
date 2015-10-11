using System.Collections.Generic;

namespace MvcSiteMapProvider.Collections.Specialized
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
