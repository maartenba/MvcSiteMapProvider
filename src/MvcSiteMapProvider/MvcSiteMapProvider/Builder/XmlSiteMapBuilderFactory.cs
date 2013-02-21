using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Globalization;
using MvcSiteMapProvider.Xml;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// An abstract factory that can be used to create new instances of <see cref="T:MvcSiteMapProvider.Builder.XmlSiteMapBuilder"/>
    /// at runtime.
    /// </summary>
    public class XmlSiteMapBuilderFactory
        : IXmlSiteMapBuilderFactory
    {
        public XmlSiteMapBuilderFactory(
            INodeKeyGenerator nodeKeyGenerator,
            IDynamicNodeBuilder dynamicNodeBuilder,
            ISiteMapNodeFactory siteMapNodeFactory
            )
        {
            if (nodeKeyGenerator == null)
                throw new ArgumentNullException("nodeKeyGenerator");
            if (dynamicNodeBuilder == null)
                throw new ArgumentNullException("dynamicNodeBuilder");
            if (siteMapNodeFactory == null)
                throw new ArgumentNullException("siteMapNodeFactory");

            this.nodeKeyGenerator = nodeKeyGenerator;
            this.dynamicNodeBuilder = dynamicNodeBuilder;
            this.siteMapNodeFactory = siteMapNodeFactory;
        }

        protected readonly INodeKeyGenerator nodeKeyGenerator;
        protected readonly IDynamicNodeBuilder dynamicNodeBuilder;
        protected readonly ISiteMapNodeFactory siteMapNodeFactory;

        #region IXmlSiteMapBuilderFactory Members

        public virtual ISiteMapBuilder Create(string xmlSiteMapFilePath, IEnumerable<string> attributesToIgnore)
        {
            var fileXmlSource = new FileXmlSource(xmlSiteMapFilePath);
            return new XmlSiteMapBuilder(
                attributesToIgnore,
                fileXmlSource,
                nodeKeyGenerator, 
                dynamicNodeBuilder, 
                siteMapNodeFactory);
        }

        #endregion
    }
}
