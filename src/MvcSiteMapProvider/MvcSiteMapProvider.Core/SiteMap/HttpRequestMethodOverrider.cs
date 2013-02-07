using System;
using System.Reflection;
using System.Web;

namespace MvcSiteMapProvider.Core.SiteMap
{
    /// <summary>
    /// Wraps an <see cref="HttpRequest"/> and overrides <see cref="HttpMethod"/> value.
    /// </summary>
    public class HttpRequestMethodOverrider : HttpRequestWrapper
    {
        private readonly string httpMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpRequest2"/> class.
        /// </summary>
        /// <param name="httpRequest">The object that this wrapper class provides access to.</param>
        /// <param name="httpMethod">The <see cref="HttpMethod"/></param>
        /// <exception cref="T:System.ArgumentNullException">
        /// 	<paramref name="httpRequest"/> is null.
        /// </exception>
        public HttpRequestMethodOverrider(HttpRequest httpRequest, string httpMethod)
            : base(httpRequest)
        {
            this.httpMethod = httpMethod;
        }

        /// <summary>
        /// Gets the HTTP data-transfer method (such as GET, POST, or HEAD) that was used by the client.
        /// </summary>
        /// <returns>
        /// The HTTP data-transfer method that was used by the client.
        /// </returns>
        public override string HttpMethod
        {
            get { return this.httpMethod ?? base.HttpMethod; }
        }
    }
}