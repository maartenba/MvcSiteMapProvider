namespace VideoSitemap
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using MvcSiteMapProvider;
    using MvcSiteMapProvider.Loader;
    using MvcSiteMapProvider.Web.Mvc;

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
                var element = new XElement(extension.NameSpace + extension.ElementName);
                extension.AddNodeContentToElement(extensibleNode, element);
                urlElement.Add(element);
            }
        }
    }
}
