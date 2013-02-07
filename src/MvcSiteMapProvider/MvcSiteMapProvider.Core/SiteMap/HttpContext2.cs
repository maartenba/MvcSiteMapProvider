using System;
using System.Web;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// HttpContext2 wrapper.
    /// </summary>
    public class HttpContext2 : HttpContextWrapper
    {
        private readonly HttpContext httpContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpContext2"/> class.
        /// </summary>
        /// <param name="httpContext">The object that this wrapper class provides access to.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="httpContext"/> is null.
        /// </exception>
        public HttpContext2(HttpContext httpContext)
            : base(httpContext)
        {
            this.httpContext = httpContext;
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
            get { return new HttpRequest2(this.httpContext.Request); }
        }
    }
}