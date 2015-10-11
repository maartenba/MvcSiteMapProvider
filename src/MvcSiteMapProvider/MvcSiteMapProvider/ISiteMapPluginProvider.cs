using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Security;
using MvcSiteMapProvider.Web.Mvc;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for plugins used by <see cref="T:MvcSiteMapProvider.SiteMap"/>.
    /// </summary>
    public interface ISiteMapPluginProvider
    {
        ISiteMapBuilder SiteMapBuilder { get; }
        IMvcResolver MvcResolver { get; }
        IAclModule AclModule { get; }
    }
}
