using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web;
using MvcSiteMapProvider.Loader;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// XmlSiteMapResult class.
    /// </summary>
    public class XmlSiteMapResult
        : ActionResult
    {
        public XmlSiteMapResult(
            int page,
            ISiteMapNode rootNode,
            IEnumerable<string> siteMapCacheKeys,
            string baseUrl,
            string siteMapUrlTemplate,
            ISiteMapLoader siteMapLoader)
        {
            if (siteMapLoader == null)
                throw new ArgumentNullException("siteMapLoader");

            this.Ns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            this.Page = page;
            this.RootNode = rootNode;
            this.SiteMapCacheKeys = siteMapCacheKeys;
            this.BaseUrl = baseUrl;
            this.SiteMapUrlTemplate = siteMapUrlTemplate;
            this.siteMapLoader = siteMapLoader;
        }

        private readonly ISiteMapLoader siteMapLoader;

        /// <summary>
        /// Maximal number of links per sitemap file.
        /// </summary>
        /// <remarks>
        /// This number should be 50000 in theory, see http://www.sitemaps.org/protocol.php#sitemapIndex_sitemap.
        /// Since sitemap files can be maximal 10MB per file and calculating the total sitemap size would degrade performance,
        /// an average cap of 35000 has been chosen.
        /// </remarks>
        private const int MaxNumberOfLinksPerFile = 35000;

        /// <summary>
        /// Source metadata
        /// </summary>
        protected Dictionary<string, object> SourceMetadata = new Dictionary<string, object> { { "HtmlHelper", typeof(XmlSiteMapResult).FullName } };

        /// <summary>
        /// Gets or sets the XML namespace.
        /// </summary>
        /// <value>The XML namespace.</value>
        protected XNamespace Ns { get; private set; }

        /// <summary>
        /// Gets or sets the root node.
        /// </summary>
        /// <value>The root node.</value>
        protected ISiteMapNode RootNode { get; private set; }

        /// <summary>
        /// Gets or sets the site map cache keys.
        /// </summary>
        /// <value>The site map cache keys.</value>
        protected IEnumerable<string> SiteMapCacheKeys { get; private set; }

        /// <summary>
        /// Gets or sets the base URL.
        /// </summary>
        /// <value>The base URL.</value>
        protected string BaseUrl { get; private set; }

        /// <summary>
        /// Gets or sets the site map URL template.
        /// </summary>
        /// <value>The site map URL template.</value>
        protected string SiteMapUrlTemplate { get; private set; }

        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>The page.</value>
        protected int Page { get; set; }


        /// <summary>
        /// Executes the sitemap index result.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="flattenedHierarchy">The flattened hierarchy.</param>
        /// <param name="flattenedHierarchyCount">The flattened hierarchy count.</param>
        protected virtual void ExecuteSitemapIndexResult(ControllerContext context, IEnumerable<ISiteMapNode> flattenedHierarchy, long flattenedHierarchyCount)
        {
            // Count the number of pages
            double numPages = Math.Ceiling((double)flattenedHierarchyCount / MaxNumberOfLinksPerFile);

            // Output content type
            context.HttpContext.Response.ContentType = "text/xml";

            // Generate sitemap sitemapindex
            var sitemapIndex = new XElement(Ns + "sitemapindex");
            sitemapIndex.Add(GenerateSiteMapIndexElements(Convert.ToInt32(numPages), BaseUrl, SiteMapUrlTemplate).ToArray());

            // Generate sitemap
            var xmlSiteMap = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                sitemapIndex);

            // Write XML
            using (Stream outputStream = RetrieveOutputStream(context))
            {
                using (var writer = XmlWriter.Create(outputStream))
                {
                    xmlSiteMap.WriteTo(writer);
                }
                outputStream.Flush();
            }
        }

        /// <summary>
        /// Executes the sitemap result.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        /// <param name="flattenedHierarchy">The flattened hierarchy.</param>
        /// <param name="flattenedHierarchyCount">The flattened hierarchy count.</param>
        /// <param name="page">The page.</param>
        protected virtual void ExecuteSitemapResult(ControllerContext context, IEnumerable<ISiteMapNode> flattenedHierarchy, long flattenedHierarchyCount, int page)
        {

            // Output content type
            context.HttpContext.Response.ContentType = "text/xml";

            // Generate URL set
            var urlSet = new XElement(Ns + "urlset");
            urlSet.Add(GenerateUrlElements(
                context,
                flattenedHierarchy.Skip((page - 1)* MaxNumberOfLinksPerFile)
                    .Take(MaxNumberOfLinksPerFile), BaseUrl).ToArray());

            // Generate sitemap
            var xmlSiteMap = new XDocument(
                new XDeclaration("1.0", "utf-8", "true"),
                urlSet);

            // Write XML
            using (Stream outputStream = RetrieveOutputStream(context))
            {
                using (var writer = XmlWriter.Create(outputStream))
                {
                    xmlSiteMap.WriteTo(writer);
                }
                outputStream.Flush();
            }
        }

        /// <summary>
        /// Enables processing of the result of an action method by a custom type that inherits from the <see cref="T:System.Web.Mvc.ActionResult"/> class.
        /// </summary>
        /// <param name="context">The context in which the result is executed. The context information includes the controller, HTTP content, request context, and route data.</param>
        public override void ExecuteResult(ControllerContext context)
        {
            var flattenedHierarchy = new List<ISiteMapNode>();

            // Flatten link hierarchy
            if (SiteMapCacheKeys.Count() > 0)
            {
                foreach (var key in SiteMapCacheKeys)
                {
                    var siteMap = siteMapLoader.GetSiteMap(key);
                    if (siteMap == null)
                    {
                        throw new UnknownSiteMapException(Resources.Messages.UnknownSiteMap);
                    }

                    flattenedHierarchy.AddRange(FlattenHierarchy(siteMap.RootNode, BaseUrl));
                }
            }
            else
            {
                flattenedHierarchy.AddRange(FlattenHierarchy(this.RootNode, BaseUrl));
            }
            var flattenedHierarchyCount = flattenedHierarchy.LongCount();

            // Determine type of sitemap to generate: sitemap index file or sitemap file
            if (flattenedHierarchyCount > MaxNumberOfLinksPerFile && Page == 0)
            {
                // Sitemap index file
                ExecuteSitemapIndexResult(context, flattenedHierarchy, flattenedHierarchyCount);
            }
            else if (flattenedHierarchyCount > MaxNumberOfLinksPerFile && Page > 0)
            {
                // Sitemap file for links of page X
                ExecuteSitemapResult(context, flattenedHierarchy, flattenedHierarchyCount, Page);
            }
            else
            {
                // Sitemap file for all links
                ExecuteSitemapResult(context, flattenedHierarchy, flattenedHierarchyCount, 1);
            }
        }

        /// <summary>
        /// Generates the sitemap index elements.
        /// </summary>
        /// <param name="numPages">The number of pages.</param>
        /// <param name="url">The URL.</param>
        /// <param name="siteMapUrlTemplate">The site map URL template.</param>
        /// <returns>The sitemap index elements.</returns>
        protected virtual IEnumerable<XElement> GenerateSiteMapIndexElements(int numPages, string url, string siteMapUrlTemplate)
        {
            // Generate elements
            for (int i = 1; i <= numPages; i++)
            {
                yield return new XElement(Ns + "sitemap",
                    new XElement(Ns + "loc", url + "/" + siteMapUrlTemplate.Replace("{page}", i.ToString())));
            }
        }

        /// <summary>
        /// Generates the URL elements.
        /// </summary>
        /// <param name="siteMapNodes">The site map nodes.</param>
        /// <param name="url">The URL.</param>
        /// <returns>The URL elements.</returns>
        protected virtual IEnumerable<XElement> GenerateUrlElements(ControllerContext context, IEnumerable<ISiteMapNode> siteMapNodes, string url)
        {
            bool skip;
            // Iterate all nodes
            foreach (var siteMapNode in siteMapNodes)
            {
                skip = false;

                // Generate element
                var siteMapNodeUrl = siteMapNode.Url;
                string nodeUrl = url + siteMapNodeUrl;
                if (siteMapNode.HasAbsoluteUrl())
                {
                    nodeUrl = siteMapNodeUrl;
                }
                if (siteMapNode.HasExternalUrl(context.HttpContext) ||
                    !String.IsNullOrEmpty(siteMapNode.CanonicalUrl) ||
                    siteMapNode.HasNoIndexAndNoFollow)
                {
                    // Skip nodes where domain doesn't match the current one 
                    // or where canonical url exists, or that
                    // have both a noindex and nofollow robots meta tag.
                    skip = true;
                }

                var urlElement = new XElement(Ns + "url",
                    new XElement(Ns + "loc", nodeUrl));

                // Generate element properties
                if (!skip)
                {
                    if (siteMapNode.LastModifiedDate > DateTime.MinValue)
                    {
                        urlElement.Add(new XElement(Ns + "lastmod", siteMapNode.LastModifiedDate.ToUniversalTime()));
                    }
                    if (siteMapNode.ChangeFrequency != ChangeFrequency.Undefined)
                    {
                        urlElement.Add(new XElement(Ns + "changefreq", siteMapNode.ChangeFrequency.ToString().ToLowerInvariant()));
                    }
                    if (siteMapNode.UpdatePriority != UpdatePriority.Undefined)
                    {
                        urlElement.Add(new XElement(Ns + "priority", (double)siteMapNode.UpdatePriority / 100));
                    }
                
                    // Return
                    yield return urlElement;
                }
            }
        }

        /// <summary>
        /// Generates flat list of SiteMapNode from SiteMap hierarchy.
        /// </summary>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="url">The URL.</param>
        /// <returns>A flat list of SiteMapNode.</returns>
        protected virtual IEnumerable<ISiteMapNode> FlattenHierarchy(ISiteMapNode startingNode, string url)
        {
            // Mvc node
            var mvcNode = startingNode;

            // Render current node?
            if (mvcNode == null || mvcNode.Clickable)
            {
                yield return startingNode;
            }
            if (startingNode.HasChildNodes)
            {
                // Make sure all child nodes are accessible prior to rendering them...
                var shouldRender = true;
                foreach (ISiteMapNode node in startingNode.ChildNodes)
                {
                    // Check visibility
                    if (node != null)
                    {
                        shouldRender = node.IsVisible(SourceMetadata);
                    }

                    // Check ACL
                    if (!node.IsAccessibleToUser())
                    {
                        shouldRender = false;
                        break;
                    }

                    // Render child nodes?
                    if (shouldRender)
                    {
                        if (node.IsAccessibleToUser())
                        {
                            foreach (var childNode in FlattenHierarchy(node, url))
                            {
                                yield return childNode;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Retrieves the output stream.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        protected virtual Stream RetrieveOutputStream(ControllerContext context)
        {
            // Output stream
            Stream outputStream = context.HttpContext.Response.OutputStream;

            // Check if output can be GZip compressed
            var headers = context.RequestContext.HttpContext.Request.Headers;
            if (headers["Accept-encoding"] != null && headers["Accept-encoding"].ToLowerInvariant().Contains("gzip"))
            {
                context.RequestContext.HttpContext.Response.AppendHeader("Content-encoding", "gzip");
                outputStream = new GZipStream(context.HttpContext.Response.OutputStream, CompressionMode.Compress);
            }
            else if (headers["Accept-encoding"] != null && headers["Accept-encoding"].ToLowerInvariant().Contains("deflate"))
            {
                context.RequestContext.HttpContext.Response.AppendHeader("Content-encoding", "deflate");
                outputStream = new DeflateStream(context.HttpContext.Response.OutputStream, CompressionMode.Compress);
            }
            return outputStream;
        }
    }
}