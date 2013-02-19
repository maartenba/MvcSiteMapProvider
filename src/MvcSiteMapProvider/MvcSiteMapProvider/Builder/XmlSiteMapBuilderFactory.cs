using System;
using System.Collections.Generic;
using MvcSiteMapProvider.Globalization;

namespace MvcSiteMapProvider.Builder
{
    /// <summary>
    /// TODO: Update summary.
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
            return new XmlSiteMapBuilder(
                xmlSiteMapFilePath, 
                attributesToIgnore, 
                nodeKeyGenerator, 
                dynamicNodeBuilder, 
                siteMapNodeFactory);
        }

        #endregion
    }
}
