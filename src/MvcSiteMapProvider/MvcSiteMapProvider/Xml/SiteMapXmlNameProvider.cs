using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace MvcSiteMapProvider.Xml
{
    /// <summary>
    /// Class that provides details of the sitemap XML element names.
    /// </summary>
    public class SiteMapXmlNameProvider
        : ISiteMapXmlNameProvider
    {
        protected const string xmlRootName = "mvcSiteMap";
        protected const string xmlNodeName = "mvcSiteMapNode";
        protected readonly XNamespace xmlSiteMapNamespace = "http://mvcsitemap.codeplex.com/schemas/MvcSiteMap-File-3.0";

        #region ISiteMapXmlNameProvider Members

        public virtual XName NodeName
        {
            get { return xmlSiteMapNamespace + xmlNodeName; }
        }

        public virtual XName RootName
        {
            get { return xmlSiteMapNamespace + xmlRootName; }
        }

        public virtual void FixXmlNamespaces(XDocument xml)
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

        #endregion
    }
}
