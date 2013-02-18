using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ICacheDependencyFactory
    {
        ICacheDependency Create();
    }
}
