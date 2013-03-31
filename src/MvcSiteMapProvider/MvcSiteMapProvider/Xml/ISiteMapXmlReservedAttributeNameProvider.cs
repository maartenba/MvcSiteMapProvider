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
        bool IsRegularAttribute(string attributeName);
        bool IsRouteAttribute(string attributeName);
    }
}
