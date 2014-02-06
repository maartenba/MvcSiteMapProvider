using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Contract for reserved XML attribute name provider for SiteMap nodes.
    /// </summary>
    public interface ISiteMapXmlReservedAttributeNameProvider
    {
        /// <summary>
        /// Determines whether the attribute with the supplied name can be added to the 
        /// <see cref="T:SiteMapNodeProvider.Collections.Specialized.IAttributeDictionary"/>.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns><c>true</c> if the attribute can be added; otherwise <c>false</c>.</returns>
        bool IsRegularAttribute(string attributeName);

        /// <summary>
        /// Determines whether the route value with the supplied name can be added to the 
        /// <see cref="T:SiteMapNodeProvider.Collections.Specialized.IRouteValueDictionary"/>.
        /// </summary>
        /// <param name="attributeName">The name of the attribute.</param>
        /// <returns><c>true</c> if the attribute can be added; otherwise <c>false</c>.</returns>
        bool IsRouteAttribute(string attributeName);
    }
}
