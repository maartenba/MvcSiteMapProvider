using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    /// <summary>
    /// MvcSiteMapNodeAttribute Definition
    /// </summary>
    public interface IMvcSiteMapNodeAttributeDefinition
    {
        /// <summary>
        /// Gets or sets the site map node attribute.
        /// </summary>
        /// <value>The site map node attribute.</value>
        IMvcSiteMapNodeAttribute SiteMapNodeAttribute { get; set; }
    }
}
