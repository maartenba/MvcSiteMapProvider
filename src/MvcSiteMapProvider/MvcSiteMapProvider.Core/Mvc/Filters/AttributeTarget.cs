using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Core.Mvc.Filters
{
    /// <summary>
    /// AttributeTarget
    /// </summary>
    public enum AttributeTarget
    {
        /// <summary>
        /// CurrentNode
        /// </summary>
        CurrentNode = 0,

        /// <summary>
        /// ParentNode
        /// </summary>
        ParentNode = 1
    }
}
