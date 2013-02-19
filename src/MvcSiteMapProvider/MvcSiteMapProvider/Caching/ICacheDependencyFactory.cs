using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Caching
{
    /// <summary>
    /// Contract for an abstract factory that creates new instances of <see cref="T:MvcSiteMapProvider.Builder.ICacheDependency"/> 
    /// at runtime.
    /// </summary>
    public interface ICacheDependencyFactory
    {
        ICacheDependency Create();
    }
}
