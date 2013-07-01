using System.Web;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// HttpContext wrapper.
    /// </summary>
    public class SiteMapHttpContext : HttpContextWrapper
    {
        private readonly HttpContext httpContext;
        private readonly ISiteMapNode node;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapHttpContext"/> class.
        /// </summary>
        /// <param name="httpContext">The object that this wrapper class provides access to.</param>
        /// <param name="node">The site map node to fake node access context for or <c>null</c>.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="httpContext"/> is null.
        /// </exception>
        public SiteMapHttpContext(HttpContext httpContext, ISiteMapNode node)
            : base(httpContext)
        {
            this.httpContext = httpContext;
            this.node = node;
        }

        /// <summary>
        /// Gets the <see cref="T:System.Web.HttpRequestBase"/> object for the current HTTP request.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The current HTTP request.
        /// </returns>
        public override HttpRequestBase Request
        {
            get { return new SiteMapHttpRequest(this.httpContext.Request, this.node); }
        }
    }
}