using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// CacheDescription class
    /// </summary>
    public class CacheDescription
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheDescription"/> class.
        /// </summary>
        /// <param name="key">The key.</param>
        public CacheDescription(string key)
        {
            this.Key = key;
            this.AbsoluteExpiration = DateTime.MaxValue;
            this.SlidingExpiration = TimeSpan.Zero;
            this.Priority = CacheItemPriority.Default;
        }

        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        /// <value>The key.</value>
        public String Key { get; set; }

        /// <summary>
        /// Gets or sets the dependencies.
        /// </summary>
        /// <value>The dependencies.</value>
        public CacheDependency Dependencies { get; set; }

        /// <summary>
        /// Gets or sets the absolute expiration.
        /// </summary>
        /// <value>The absolute expiration.</value>
        public DateTime AbsoluteExpiration { get; set; }

        /// <summary>
        /// Gets or sets the sliding expiration.
        /// </summary>
        /// <value>The sliding expiration.</value>
        public TimeSpan SlidingExpiration { get; set; }

        /// <summary>
        /// Gets or sets the priority.
        /// </summary>
        /// <value>The priority.</value>
        public CacheItemPriority Priority { get; set; }
    }
}
