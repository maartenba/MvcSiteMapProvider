using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// Abstract factory to assist with the creation of XmlSiteMapBuilder for DI containers 
    /// that don't support injection of a partial list of constructor parameters.
    /// </summary>
    public class XmlSiteMapBuilderFactory
        : IXmlSiteMapBuilderFactory
    {
        public XmlSiteMapBuilderFactory(
            ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider,
            ISiteMapXmlNameProvider xmlNameProvider,
            ISiteMapAssemblyService siteMapAssemblyService
            )
        {
            if (reservedAttributeNameProvider == null)
                throw new ArgumentNullException("reservedAttributeNameProvider");
            if (xmlNameProvider == null)
                throw new ArgumentNullException("xmlNameProvider");
            if (siteMapAssemblyService == null)
                throw new ArgumentNullException("siteMapAssemblyService");

            this.reservedAttributeNameProvider = reservedAttributeNameProvider;
            this.xmlNameProvider = xmlNameProvider;
            this.siteMapAssemblyService = siteMapAssemblyService;
        }
        protected readonly ISiteMapXmlReservedAttributeNameProvider reservedAttributeNameProvider;
        protected readonly ISiteMapXmlNameProvider xmlNameProvider;
        protected readonly ISiteMapAssemblyService siteMapAssemblyService;

        #region IXmlSiteMapBuilderFactory Members

        public virtual ISiteMapBuilder Create(IXmlSource xmlSource)
        {
            return new XmlSiteMapBuilder(
                xmlSource,
                this.reservedAttributeNameProvider,
                this.xmlNameProvider,
                this.siteMapAssemblyService);
        }

        #endregion
    }
}
