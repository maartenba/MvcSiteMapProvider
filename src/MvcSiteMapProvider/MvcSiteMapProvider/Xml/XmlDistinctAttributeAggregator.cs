using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Parses and aggregates the values of a named attribute in an XML source.
    /// </summary>
    public class XmlDistinctAttributeAggregator
    {
        public XmlDistinctAttributeAggregator(
            ISiteMapXmlNameProvider xmlNameProvider
            )
        {
            if (xmlNameProvider == null)
                throw new ArgumentNullException("xmlNameProvider");
            this.xmlNameProvider = xmlNameProvider;
        }

        private readonly ISiteMapXmlNameProvider xmlNameProvider;

        public IList<string> GetAttributeValues(IXmlSource xmlSource, string attributeName)
        {
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
