using MvcSiteMapProvider.Web.UrlResolver;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for plugins used by <see cref="T:MvcSiteMapProvider.SiteMapNode"/>.
    /// </summary>
    public interface ISiteMapNodePluginProvider
    {
        IDynamicNodeProviderStrategy DynamicNodeProviderStrategy { get; }
        ISiteMapNodeUrlResolverStrategy UrlResolverStrategy { get; }
        ISiteMapNodeVisibilityProviderStrategy VisibilityProviderStrategy { get; }
    }
}
