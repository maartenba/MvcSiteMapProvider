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
        protected const string xmlRootName = "mvcSiteMap";
        protected const string xmlNodeName = "mvcSiteMapNode";
        protected readonly XNamespace xmlSiteMapNamespace = "http://mvcsitemap.codeplex.com/schemas/MvcSiteMap-File-3.0";

        public IList<string> GetAttributeValues(string filePath, string attributeName)
        {
            var xmlSource = new FileXmlSource(filePath);
            var xml = xmlSource.GetXml();
            FixXmlNamespaces(xml);

            var result = xml.Element(xmlSiteMapNamespace + xmlRootName)
                            .Descendants()
                            .Select(e => (string)e.Attribute(attributeName))
                            .Where(x => x != null)
                            .Distinct()
                            .ToList();

            return result;
        }

        // TODO: Factor this out into another class (it is shared with XmlSiteMapBuilder)
        protected virtual void FixXmlNamespaces(XDocument xml)
        {
            // If no namespace is present (or the wrong one is present), replace it
            foreach (var node in xml.Descendants())
            {
                if (string.IsNullOrEmpty(node.Name.Namespace.NamespaceName) || node.Name.Namespace != this.xmlSiteMapNamespace)
                {
                    node.Name = XName.Get(node.Name.LocalName, this.xmlSiteMapNamespace.ToString());
                }
            }
        }
    }
}
