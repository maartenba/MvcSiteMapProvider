using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapBuilderSet
    {
        string Name { get; }
        ISiteMapBuilder Builder { get; }
    }
}
