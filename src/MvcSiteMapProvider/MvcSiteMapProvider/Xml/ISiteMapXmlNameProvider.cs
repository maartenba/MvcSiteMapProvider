using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Contract for class that provides details of the sitemap XML element names.
    /// </summary>
    public interface ISiteMapXmlNameProvider
    {
        XName NodeName { get; }
        XName RootName { get; }
        void FixXmlNamespaces(XDocument xml);
    }
}
