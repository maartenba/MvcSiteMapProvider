using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.DI
{
    /// <summary>
    /// Parses and aggregates the values of a named attribute in an XML file.
    /// </summary>
    public class XmlDistinctAttributeAggregator
    {
        public IList<string> GetAttributeValues(string filePath, string attributeName)
        {
            var xmlNameProvider = new SiteMapXmlNameProvider();
            var xmlSource = new FileXmlSource(filePath);
            var xml = xmlSource.GetXml();
            xmlNameProvider.FixXmlNamespaces(xml);

            var result = xml.Element(xmlNameProvider.RootName)
                            .Descendants()
                            .Select(e => (string)e.Attribute(attributeName))
                            .Where(x => x != null)
                            .Distinct()
                            .ToList();

            return result;
        }
    }
}
