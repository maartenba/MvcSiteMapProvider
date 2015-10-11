using MvcSiteMapProvider.Xml;
using System;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Abstract factory to assist with the creation of XmlSiteMapNodeProviderFactory for DI containers 
    /// that don't support injection of a partial list of constructor parameters. Without using this 
    /// class, DI configuration code for those containers is very brittle.
    /// </summary>
    public class XmlSiteMapNodeProviderFactory
    {
        public XmlSiteMapNodeProviderFactory(
            ISiteMapXmlNameProvider xmlNameProvider
            )
        {
            if (xmlNameProvider == null)
                throw new ArgumentNullException("xmlNameProvider");

            this.xmlNameProvider = xmlNameProvider;
        }
        protected readonly ISiteMapXmlNameProvider xmlNameProvider;

        public virtual XmlSiteMapNodeProvider Create(IXmlSource xmlSource, bool includeRootNode, bool useNestedDynamicNodeRecursion)
        {
            return new XmlSiteMapNodeProvider(includeRootNode, useNestedDynamicNodeRecursion, xmlSource, this.xmlNameProvider);
        }

        public virtual XmlSiteMapNodeProvider Create(IXmlSource xmlSource, bool includeRootNode)
        {
            return this.Create(xmlSource, includeRootNode, false);
        }

        public virtual XmlSiteMapNodeProvider Create(IXmlSource xmlSource)
        {
            return this.Create(xmlSource, true, false);
        }
    }
}
