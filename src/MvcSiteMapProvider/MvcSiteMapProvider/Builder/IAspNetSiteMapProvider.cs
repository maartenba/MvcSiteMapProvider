using System.Web;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for ASP.NET classic SiteMapProvider. Implement this
    /// interface to retrieve the SiteMapProvider by name or other means.
    /// </summary>
    public interface IAspNetSiteMapProvider
    {
        SiteMapProvider GetProvider();
    }
}
