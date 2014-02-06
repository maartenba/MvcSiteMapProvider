using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    public interface IDynamicSiteMapNodeBuilderFactory
    {
        IDynamicSiteMapNodeBuilder Create(ISiteMap siteMap);
    }
}
