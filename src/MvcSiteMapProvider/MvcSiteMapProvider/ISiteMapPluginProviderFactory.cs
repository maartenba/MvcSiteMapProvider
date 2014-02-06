using System;
using MvcSiteMapProvider.Web.Mvc;
using MvcSiteMapProvider.Builder;

namespace MvcSiteMapProvider
{
    /// <summary>
    /// Contract for an abstract factory that can be used to create new instances of 
    /// <see cref="T:MvcSiteMapProvider.ISiteMapPluginProvider"/> at runtime.
    /// </summary>
    public interface ISiteMapPluginProviderFactory
    {
        ISiteMapPluginProvider Create(ISiteMapBuilder siteMapBuilder, IMvcResolver mvcResolver);
    }
}
