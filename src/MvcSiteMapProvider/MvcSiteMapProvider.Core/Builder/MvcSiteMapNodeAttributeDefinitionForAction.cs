using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MvcSiteMapProvider.Core.Builder
{
    /// <summary>
    /// MvcSiteMapNodeAttributeDefinition for Action
    /// </summary>
    public class MvcSiteMapNodeAttributeDefinitionForAction
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

        /// <summary>
        /// Gets or sets the action method info.
        /// </summary>
        /// <value>The action method info.</value>
        public MethodInfo ActionMethodInfo { get; set; }
    }
}
