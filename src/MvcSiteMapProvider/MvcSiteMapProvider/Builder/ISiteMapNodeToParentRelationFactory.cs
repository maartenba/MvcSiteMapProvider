using System;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeToParentRelation"/>.
    /// </summary>
    public interface ISiteMapNodeToParentRelationFactory
    {
        ISiteMapNodeToParentRelation Create(string parentKey, ISiteMapNode node, string sourceName);
    }
}
