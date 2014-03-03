using System;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider.Builder
{
    public interface IDynamicSiteMapNodeBuilderFactory
    {
        IDynamicSiteMapNodeBuilder Create(ISiteMap siteMap, ICultureContext cultureContext);
    }
}
