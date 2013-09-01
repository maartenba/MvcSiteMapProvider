using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.IO.Compression;
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

        protected readonly ISiteMapLoader siteMapLoader;
        protected readonly List<string> duplicateUrlCheck = new List<string>();

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
            sitemapIndex.Add(GenerateSiteMapIndexElements(Convert.ToInt32(numPages), this.BaseUrl, SiteMapUrlTemplate).ToArray());

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
            var flattenedHierarchy = new HashSet<ISiteMapNode>();

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

                    foreach (var item in FlattenHierarchy(siteMap.RootNode, context))
                    {
                        flattenedHierarchy.Add(item);
                    }
                }
            }
            else
            {
                foreach (var item in FlattenHierarchy(this.RootNode, context))
                {
                    flattenedHierarchy.Add(item);
                }
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
        /// <param name="siteMapUrlTemplate">The site map URL template.</param>
        /// <returns>The sitemap index elements.</returns>
        protected virtual IEnumerable<XElement> GenerateSiteMapIndexElements(int numPages, string baseUrl, string siteMapUrlTemplate)
        {
            // Generate elements
            for (int i = 1; i <= numPages; i++)
            {
                var pageUrl = baseUrl + "/" + siteMapUrlTemplate.Replace("{page}", i.ToString());
                yield return new XElement(Ns + "sitemap", new XElement(Ns + "loc", pageUrl));
            }
        }

        /// <summary>
        /// Generates the URL elements.
        /// </summary>
        /// <param name="siteMapNodes">The site map nodes.</param>
        /// <param name="url">The URL.</param>
        /// <returns>The URL elements.</returns>
        protected virtual IEnumerable<XElement> GenerateUrlElements(ControllerContext context, IEnumerable<ISiteMapNode> siteMapNodes, string baseUrl)
        {
            // Iterate all nodes
            foreach (var siteMapNode in siteMapNodes)
            {
                // Generate element
                var nodeUrl = this.GetAbsoluteUrl(siteMapNode);
                var urlElement = new XElement(Ns + "url",
                    new XElement(Ns + "loc", nodeUrl));

                // Generate element properties
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
                    urlElement.Add(new XElement(Ns + "priority", string.Format("{0:0.0}", ((double)siteMapNode.UpdatePriority / 100))));
                }

                // Return
                yield return urlElement;
            }
        }

        /// <summary>
        /// Generates flat list of SiteMapNode from SiteMap hierarchy.
        /// </summary>
        /// <param name="startingNode">The starting node.</param>
        /// <param name="url">The URL.</param>
        /// <returns>A flat list of SiteMapNode.</returns>
        protected virtual IEnumerable<ISiteMapNode> FlattenHierarchy(ISiteMapNode startingNode, ControllerContext context)
        {
            // Inaccessible - don't process current node or any descendant nodes.
            if (startingNode.IsAccessibleToUser())
            {
                if (this.ShouldNodeRender(startingNode, context))
                {
                    yield return startingNode;
                }
                if (startingNode.HasChildNodes)
                {
                    // Make sure all child nodes are accessible prior to rendering them...
                    foreach (ISiteMapNode node in startingNode.ChildNodes)
                    {
                        foreach (var childNode in FlattenHierarchy(node, context))
                        {
                            yield return childNode;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks all rules to determine if the current node should render in the sitemap.
        /// </summary>
        /// <param name="node">The node</param>
        /// <param name="context">The controller context</param>
        /// <returns><b>true</b> if the current node should be rendered; otherwise<b>false</b>.</returns>
        protected virtual bool ShouldNodeRender(ISiteMapNode node, ControllerContext context)
        {
            return node.Clickable &&
                node.IsVisible(SourceMetadata) &&
                !node.HasExternalUrl(context.HttpContext) &&
                String.IsNullOrEmpty(node.CanonicalUrl) &&
                !node.HasNoIndexAndNoFollow &&
                !this.IsDuplicateUrl(node);
        }

        /// <summary>
        /// Gets the absolute URL for a node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns>The absolute URL.</returns>
        protected virtual string GetAbsoluteUrl(ISiteMapNode node)
        {
            var nodeUrl = node.Url;
            var result = nodeUrl;
            if (!node.HasAbsoluteUrl())
            {
                result = this.BaseUrl + nodeUrl;
            }
            return result;
        }

        /// <summary>
        /// Determines whether the URL is already included in the sitemap.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns><b>true</b> if the URL of the node is a duplicate; otherwise <b>false</b>.</returns>
        protected virtual bool IsDuplicateUrl(ISiteMapNode node)
        {
            var absoluteUrl = this.GetAbsoluteUrl(node);
            var isDuplicate = this.duplicateUrlCheck.Contains(absoluteUrl);
            if (!isDuplicate)
            {
                this.duplicateUrlCheck.Add(absoluteUrl);
            }
            return isDuplicate;
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