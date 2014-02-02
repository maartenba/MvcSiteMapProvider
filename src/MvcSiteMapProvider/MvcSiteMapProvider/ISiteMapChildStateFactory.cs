using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections.Specialized;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of types required by the <see cref="T:MvcSiteMapProvider.SiteMap"/>
    /// at runtime.
    /// </summary>
    public interface ISiteMapChildStateFactory
    {
        IDictionary<ISiteMapNode, ISiteMapNodeCollection> CreateChildNodeCollectionDictionary();
        IDictionary<string, ISiteMapNode> CreateKeyDictionary();
        IDictionary<ISiteMapNode, ISiteMapNode> CreateParentNodeDictionary();
        IDictionary<string, ISiteMapNode> CreateUrlDictionary();
        ISiteMapNodeCollection CreateSiteMapNodeCollection();
        ISiteMapNodeCollection CreateLockableSiteMapNodeCollection(ISiteMap siteMap);
        ISiteMapNodeCollection CreateReadOnlySiteMapNodeCollection(ISiteMapNodeCollection siteMapNodeCollection);
        ISiteMapNodeCollection CreateEmptyReadOnlySiteMapNodeCollection();
    }
}
