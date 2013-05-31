namespace MvcSiteMapProvider
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using Loader;
    using Web.Mvc;

    public class ExtensibleXmlSiteMapResult : XmlSiteMapResult
    {
        private readonly IEnumerable<ISiteMapResultExtension> _extensions;

        public ExtensibleXmlSiteMapResult(
            int page,
            ISiteMapNode rootNode,
            IEnumerable<string> siteMapCacheKeys,
            string baseUrl,
            string siteMapUrlTemplate,
            ISiteMapLoader siteMapLoader,
            IEnumerable<ISiteMapResultExtension> extensions)
            : base(page, rootNode, siteMapCacheKeys, baseUrl, siteMapUrlTemplate, siteMapLoader)
        {
            _extensions = extensions ?? new List<ISiteMapResultExtension>();
        }

        protected override void AddDataToUrlElement(ISiteMapNode siteMapNode, XElement urlElement)
        {
            base.AddDataToUrlElement(siteMapNode, urlElement);
            var extensibleNode = siteMapNode as IExtensibleSiteMapNode;

            foreach (var extension in _extensions)
            {
                extension.AddNodeContentToElement(extensibleNode, urlElement);
            }
        }
    }
}
