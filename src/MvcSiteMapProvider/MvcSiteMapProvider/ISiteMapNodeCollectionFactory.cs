using System;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for the abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.ISiteMapNodeCollection"/> at runtime.
    /// </summary>
    public interface ISiteMapNodeCollectionFactory
    {
        ISiteMapNodeCollection Create();
        ISiteMapNodeCollection CreateLockable(ISiteMap siteMap);
        ISiteMapNodeCollection CreateReadOnly(ISiteMapNodeCollection siteMapNodeCollection);
        ISiteMapNodeCollection CreateEmptyReadOnly();
    }
}
