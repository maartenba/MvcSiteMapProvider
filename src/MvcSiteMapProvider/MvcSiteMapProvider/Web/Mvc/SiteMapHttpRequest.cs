using System;
using System.Web;

namespace MvcSiteMapProvider.Web.Mvc
{
    /// <summary>
    /// HttpRequest wrapper.
    /// </summary>
    public class SiteMapHttpRequest 
        : HttpRequestWrapper
    {
        private readonly ISiteMapNode node_;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapHttpRequest"/> class.
        /// </summary>
        /// <param name="httpRequest">The object that this wrapper class provides access to.</param>
        /// <param name="node">The site map node to fake node access context for or <c>null</c>.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="httpRequest"/> is null.
        /// </exception>
        public SiteMapHttpRequest(HttpRequest httpRequest, ISiteMapNode node)
            : base(httpRequest)
        {
            node_ = node;
        }

        /// <summary>
        /// Gets the virtual path of the application root and makes it relative by using the tilde (~) notation for the application root (as in "~/page.aspx").
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The virtual path of the application root for the current request with the tilde operator added.
        /// </returns>
        public override string AppRelativeCurrentExecutionFilePath
        {
            get
            {
                return VirtualPathUtility.ToAppRelative(this.CurrentExecutionFilePath);
            }
        }

        /// <summary>
        /// Gets the virtual path of the current request.
        /// </summary>
        /// <value></value>
        /// <returns>
        /// The virtual path of the page handler that is currently executing.
        /// </returns>
        public override string CurrentExecutionFilePath
        {
            get { return base.FilePath; }
        }

        /// <summary>
        /// Gets the HTTP data-transfer method (such as GET, POST, or HEAD) that was used by the client.
        /// </summary>
        /// <returns>
        /// The HTTP data-transfer method that was used by the client.
        /// </returns>
        public override string HttpMethod
        {
            get
            {
                const StringComparison comparison = StringComparison.OrdinalIgnoreCase;
                var replaceMethod = node_ != null &&
                                    !String.IsNullOrWhiteSpace(node_.HttpMethod) &&
                                    !String.Equals(node_.HttpMethod, "*", comparison);
                return replaceMethod
                    ? node_.HttpMethod.Split(',')[0]
                    : base.HttpMethod;
            }
        }
    }
}