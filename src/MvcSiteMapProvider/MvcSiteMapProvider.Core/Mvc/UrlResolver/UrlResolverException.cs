#region Using directives

using System;

#endregion

namespace MvcSiteMapProvider.Core.Mvc.UrlResolver
{
    /// <summary>
    /// UrlResolverException
    /// </summary>
    public class UrlResolverException : Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UrlResolverException()
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        public UrlResolverException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="innerException">Inner Exception</param>
        public UrlResolverException(string message, Exception innerException) : base(message, innerException) { }
    }

}
