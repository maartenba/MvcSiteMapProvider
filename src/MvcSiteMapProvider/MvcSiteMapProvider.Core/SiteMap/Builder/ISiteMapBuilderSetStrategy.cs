using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapBuilderSetStrategy
    {
        ISiteMapBuilder GetBuilder(string name);
    }
}
