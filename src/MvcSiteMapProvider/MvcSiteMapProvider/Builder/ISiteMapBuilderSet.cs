using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MvcSiteMapProvider.Caching;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapBuilderSet
    {
        string Name { get; }
        ISiteMapBuilder Builder { get; }
        ICacheDependency CacheDependency { get; }

        /// <summary>
        /// Determines whether the provider instance matches the name
        /// </summary>
        /// <param name="providerName">The name of the dynamic node provider. This can be any string, but for backward compatibility the type name can be used.</param>
        /// <returns>
        /// True if the provider name matches.
        /// </returns>
        bool AppliesTo(string builderSetName);
    }
}
