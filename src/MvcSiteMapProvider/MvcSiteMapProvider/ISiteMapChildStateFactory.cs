using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Collections.Specialized;
using MvcSiteMapProvider.Matching;

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
        IDictionary<IUrlKey, ISiteMapNode> CreateUrlDictionary();
        IUrlKey CreateUrlKey(ISiteMapNode node);
        IUrlKey CreateUrlKey(string relativeOrAbsoluteUrl, string hostName);
        ISiteMapNodeCollection CreateSiteMapNodeCollection();
        ISiteMapNodeCollection CreateLockableSiteMapNodeCollection(ISiteMap siteMap);
        ISiteMapNodeCollection CreateReadOnlySiteMapNodeCollection(ISiteMapNodeCollection siteMapNodeCollection);
        ISiteMapNodeCollection CreateEmptyReadOnlySiteMapNodeCollection();
    }
}
