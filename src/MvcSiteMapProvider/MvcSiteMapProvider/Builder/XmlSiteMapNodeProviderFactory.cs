using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Xml;
using MvcSiteMapProvider.Collections.Specialized;

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

        public virtual XmlSiteMapNodeProvider Create(IXmlSource xmlSource, bool includeRootNode)
        {
            return new XmlSiteMapNodeProvider(includeRootNode, xmlSource, this.xmlNameProvider);
        }

        public virtual XmlSiteMapNodeProvider Create(IXmlSource xmlSource)
        {
            return this.Create(xmlSource, true);
        }
    }
}
