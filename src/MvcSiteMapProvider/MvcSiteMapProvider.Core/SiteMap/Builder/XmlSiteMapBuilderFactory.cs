// -----------------------------------------------------------------------
// <copyright file="XmlSiteMapBuilderFactory.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace MvcSiteMapProvider.Core.SiteMap.Builder
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using MvcSiteMapProvider.Core.Globalization;

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

        public ISiteMapBuilder Create(string xmlSiteMapFilePath, IEnumerable<string> attributesToIgnore)
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
