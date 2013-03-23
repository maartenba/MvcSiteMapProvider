using System;
using MvcSiteMapProvider.Builder;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Security;

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
