using System;

namespace MvcSiteMapProvider.Core.Mvc.Filters
{
    /// <summary>
    /// SiteMapTitleTarget
    /// </summary>
    [Obsolete("This attribute is obsolete. Use AttributeTarget instead.")]
    public enum SiteMapTitleTarget
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
