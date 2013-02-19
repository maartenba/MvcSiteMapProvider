using System;
using System.Collections.Generic;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Contract for abstract factory that can provide instances of <see cref="T:MvcSiteMapProvider.Builder.ISiteMapBuilder"/>
    /// at runtime.
    /// </summary>
    public interface IReflectionSiteMapBuilderFactory
    {
        ISiteMapBuilder Create(
            IEnumerable<String> includeAssemblies,
            IEnumerable<String> excludeAssemblies);
    }
}
