using System;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for an abstract factory that creates instances of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapNodeHelper"/>.
    /// </summary>
    public interface ISiteMapNodeHelperFactory
    {
        ISiteMapNodeHelper Create(ISiteMap siteMap, ICultureContext cultureContext);
    }
}
