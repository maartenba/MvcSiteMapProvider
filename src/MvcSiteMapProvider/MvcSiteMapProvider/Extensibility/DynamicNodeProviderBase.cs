using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Extensibility
{
    /// <summary>
    /// DynamicNodeProviderBase class
    /// </summary>
    public abstract class DynamicNodeProviderBase
        : IDynamicNodeProvider
    {
        #region IDynamicNodeProvider Members

        /// <summary>
        /// Gets the dynamic node collection.
        /// </summary>
        /// <returns>A dynamic node collection.</returns>
        public abstract IEnumerable<DynamicNode> GetDynamicNodeCollection();

        /// <summary>
        /// Gets a cache description for the dynamic node collection
        /// or null if there is none.
        /// </summary>
        /// <returns>
        /// A cache description represented as a <see cref="CacheDescription"/> instance .
        /// </returns>
        public virtual CacheDescription GetCacheDescription()
        {
            return null;
        }

        #endregion
    }
}
