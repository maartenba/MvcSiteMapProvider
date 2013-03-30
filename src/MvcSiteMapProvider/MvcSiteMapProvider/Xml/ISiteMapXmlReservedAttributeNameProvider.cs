using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface ISiteMapXmlReservedAttributeNameProvider
    {
        bool IsRegularAttribute(string attributeName);
        bool IsRouteAttribute(string attributeName);
    }
}
