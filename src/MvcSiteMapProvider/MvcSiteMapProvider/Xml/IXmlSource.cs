using System;
using System.Xml.Linq;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Contract for providing XDocument instances from various sources.
    /// </summary>
    public interface IXmlSource
    {
        XDocument GetXml();
    }
}
