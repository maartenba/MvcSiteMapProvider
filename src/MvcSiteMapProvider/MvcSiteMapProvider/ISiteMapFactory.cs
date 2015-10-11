using MvcSiteMapProvider.Builder;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.ISiteMapFactory"/> at runtime.
    /// </summary>
    public interface ISiteMapFactory
    {
        ISiteMap Create(ISiteMapBuilder siteMapBuilder, ISiteMapSettings siteMapSettings);
    }
}
