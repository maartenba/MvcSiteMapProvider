using System;
using MvcSiteMapProvider.Web;

namespace MvcSiteMapProvider.Matching
{
    /// <summary>
    /// An abstract factory that creates new instances of 
    /// <see cref="T:MvcSiteMapProvider.Matching.IUrlKey"/> at runtime.
    /// </summary>
    public class UrlKeyFactory
        : IUrlKeyFactory
    {
        public UrlKeyFactory(
            IUrlPath urlPath
            )
        {
            if (urlPath == null)
                throw new ArgumentNullException("urlPath");

            this.urlPath = urlPath;
        }
        private readonly IUrlPath urlPath;

        public IUrlKey Create(ISiteMapNode node)
        {
            return new SiteMapNodeUrlKey(node, this.urlPath);
        }

        public IUrlKey Create(string relativeOrAbsoluteUrl, string hostName)
        {
            return new RequestUrlKey(relativeOrAbsoluteUrl, hostName, this.urlPath);
        }
    }
}
