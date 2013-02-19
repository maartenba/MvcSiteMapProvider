using System;

namespace MvcSiteMapProvider.Loader
{
    /// <summary>
    /// Contract for abstract factory that can provide instances of <see cref="T:MvcSiteMapProvider.Loader.ISiteMapLoader"/>
    /// at runtime.
    /// </summary>
    public interface ISiteMapLoaderFactory
    {
        ISiteMapLoader Create();
    }
}
