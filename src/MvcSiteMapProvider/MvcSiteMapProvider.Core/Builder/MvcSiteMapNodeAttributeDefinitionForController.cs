using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Core.Builder
{
    /// <summary>
    /// MvcSiteMapNodeAttributeDefinition for Controller
    /// </summary>
    public class MvcSiteMapNodeAttributeDefinitionForController
        : IMvcSiteMapNodeAttributeDefinition
    {
        #region IMvcSiteMapNodeAttributeDefinition Members

        /// <summary>
        /// Gets or sets the site map node attribute.
        /// </summary>
        /// <value>The site map node attribute.</value>
        public IMvcSiteMapNodeAttribute SiteMapNodeAttribute { get; set; }

        #endregion

        /// <summary>
        /// Gets or sets the type of the controller.
        /// </summary>
        /// <value>The type of the controller.</value>
        public Type ControllerType { get; set; }
    }
}
