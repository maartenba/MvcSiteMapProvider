using System;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeParentMap"/>.
    /// </summary>
    public interface ISiteMapNodeParentMapFactory
    {
        ISiteMapNodeParentMap Create(string parentKey, ISiteMapNode node, string sourceName);
    }
}
